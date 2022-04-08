using SMP.Dominio.Controlador;
using SMP.Dominio.Model;
using System.Collections;
using System.Xml.Linq;

namespace SMP.Dominio
{
	public class FichaCadastroIndividual
	{
		private PessoaModel ModelPessoa { get; set; }

		private static XNamespace NS_2 = "http://esus.ufsc.br/dadoinstalacao";
		private static XNamespace NS_3 = "http://esus.ufsc.br/dadotransporte";
		private static XNamespace NS_4 = "http://esus.ufsc.br/cadastroindividua";
		private static XAttribute ATT_2 = new XAttribute(XNamespace.Xmlns + "ns2", NS_2);
		private static XAttribute ATT_3 = new XAttribute(XNamespace.Xmlns + "ns3", NS_3);
		private static XAttribute ATT_4 = new XAttribute(XNamespace.Xmlns + "ns4", NS_4);
		private static int VERSAO_MAJOR = 4;
		private static int VERSAO_MINOR = 2;
		private static int VERSAO_REVISION = 1;

		public async Task<XElement> ObterXml(string cpf)
		{
			ModelPessoa = await new ControladorPessoa().ObterPessoaPeloCPF(cpf);

			XElement dadoTransporteTransportXml = new XElement(NS_3 + "dadoTransporteTransportXml", ATT_2, ATT_3, ATT_4,
				new XElement("uuidDadoSerializado", ModelPessoa.GuidPessoa),
				new XElement("tipoDadoSerializado", 2),
				new XElement("codIbge", ModelPessoa.CodMunicipioIbge),
				new XElement("cnesDadoSerializado", ModelPessoa.CnesReferencia),
				new XElement("ineDadoSerializado", string.Empty),
				new XElement("numLote", string.Empty),
				ObterCadastroIndividualTransport(),
				ObterRemetente("remetente"),
				ObterRemetente("originadora"),
				ObterVersao()
				);

			return dadoTransporteTransportXml;
		}

		private XElement ObterCadastroIndividualTransport()
		{
			XElement saidaCidadaoCadastro = new XElement("saidaCidadaoCadastro");
			XElement statusTermoRecusaCadastroIndividualAtencaoBasica = new XElement("statusTermoRecusaCadastroIndividualAtencaoBasica", false);
			XElement tpCdsOrigem = new XElement("tpCdsOrigem", 3);
			XElement uuid = new XElement("uuid", ModelPessoa.GuidPessoa);
			XElement uuidFichaOriginadora = new XElement("uuidFichaOriginadora", ModelPessoa.GuidPessoa);

			XElement cadastroIndividualTransport = new XElement(NS_4 + "cadastroIndividualTransport",
				ObterIdentificacaoUsuarioCidadao(),
				ObterInformacoesSocioDemograficas(),
				ObterCondicoesDeSaude(),
				saidaCidadaoCadastro,
				statusTermoRecusaCadastroIndividualAtencaoBasica,
				tpCdsOrigem,
				uuid,
				uuidFichaOriginadora,
				ObterHeader());

			return cadastroIndividualTransport;
		}

		private XElement ObterIdentificacaoUsuarioCidadao()
		{
			var ModelDadosCadastrais = Utilitarios.ConverterPara<DadosCadastraisModel>(ModelPessoa);
			XElement identificacaoUsuarioCidadao = ObterXElement(ModelDadosCadastrais, "identificacaoUsuarioCidadao");
			return identificacaoUsuarioCidadao;
		}
		private XElement ObterInformacoesSocioDemograficas()
		{
			var ModelDadosSocioDemograficos = Utilitarios.ConverterPara<DadosSocioDemograficosModel>(ModelPessoa);
			XElement informacoesSocioDemograficas = ObterXElement(ModelDadosSocioDemograficos, "informacoesSocioDemograficas");
			return informacoesSocioDemograficas;
		}
		private XElement ObterCondicoesDeSaude()
		{
			var ModelDadosCondicoesSaude = Utilitarios.ConverterPara<DadosCondicoesSaudeModel>(ModelPessoa);
			XElement condicoesDeSaude = ObterXElement(ModelDadosCondicoesSaude, "condicoesDeSaude");
			return condicoesDeSaude;
		}

		private XElement ObterHeader()
		{
			XElement profissionalCNS = new XElement("profissionalCNS", "[valor do CNS]");
			XElement cboCodigo_2002 = new XElement("cboCodigo_2002", "[valor do CBO]");
			XElement cnes = new XElement("cnes", ModelPessoa.CnesReferencia);
			XElement ine = new XElement("ine", "[valor do ine]");

			long dataAtendimentoEpoch = Utilitarios.ConverterDataEpoch((DateTime)(ModelPessoa?.DataUltimaAtualizaco ?? ModelPessoa.DataCriacao));

			XElement dataAtendimento = new XElement("dataAtendimento", dataAtendimentoEpoch);
			XElement codigoIbgeMunicipio = new XElement("codigoIbgeMunicipio", ModelPessoa.CodMunicipioIbge);

			XElement headerTransport = new XElement("headerTransport", profissionalCNS, cboCodigo_2002, cnes, ine, dataAtendimento, codigoIbgeMunicipio);

			return headerTransport;
		}

		private XElement ObterRemetente(string atributo)
		{
			XElement contraChave = new XElement("contraChave", "TREINAMENTO");
			XElement uuidInstalacao = new XElement("uuidInstalacao", "TREINAMENTO");
			XElement cpfOuCnpj = new XElement("cpfOuCnpj", "Valor do cpfOuCnpj");
			XElement nomeOuRazaoSocial = new XElement("nomeOuRazaoSocial", "ADMINISTRADOR INSTALAÇÃO");
			XElement versaoSistema = new XElement("versaoSistema", "3.2.18");
			XElement nomeBancoDados = new XElement("nomeBancoDados", "Oracle");

			XElement retorno = new XElement(NS_2 + atributo,
				contraChave,
				uuidInstalacao,
				cpfOuCnpj,
				nomeOuRazaoSocial,
				versaoSistema,
				nomeBancoDados);

			return retorno;
		}

		private XElement ObterVersao()
		{
			XElement versao = new XElement("versao",
				new XAttribute("major", VERSAO_MAJOR),
				new XAttribute("minor", VERSAO_MINOR),
				new XAttribute("revision", VERSAO_REVISION)
				);

			return versao;
		}

		private XElement ObterXElement(object model, string node)
		{
			XElement retorno = new XElement(node);

			var properties = model.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(ESUSAttribute), false));
			foreach (var item in properties)
			{
				var value = item.GetValue(model);

				if (value != null)
				{
					if (value.GetType() == typeof(DateTime))
					{
						value = Utilitarios.ConverterDataEpoch((DateTime)value);
					}

					string name = string.Empty;

					var attributes = (ESUSAttribute[])item.GetCustomAttributes(typeof(ESUSAttribute), false);
					foreach (var attribute in attributes)
					{
						name = attribute.Nome;
					}

					if (value.GetType().IsGenericType == true)
					{
						foreach (var listitem in value as IEnumerable)
						{
							XElement child = new XElement(name, listitem);
							retorno.Add(child);
						}
					}
					else
					{
						XElement child = new XElement(name, value);
						retorno.Add(child);
					}
				}
			}

			return retorno;
		}
	}
}
