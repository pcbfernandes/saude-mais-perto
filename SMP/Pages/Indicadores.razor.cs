using Microsoft.AspNetCore.Components;
using SMP.Dominio;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;

namespace SMP.Pages
{
	public class IndicadoresBase : ComponentBase
	{
		public List<ModelData> ValoresSexo { get; set; } = new List<ModelData>();
		public List<ModelData> ValoresIdade { get; set; } = new List<ModelData>();
		public List<ModelData> ValoresFumante { get; set; } = new List<ModelData>();
		public List<ModelData> ValoresResponsavelFamilia { get; set; } = new List<ModelData>();
		public List<ModelData> ValoresTeveCOVID19 { get; set; } = new List<ModelData>();
		public List<ModelData> ValoresCodSituacaoMercado { get; set; } = new List<ModelData>();
		public Dictionary<string, List<ModelData>> ListaIndicadores { get; set; } = new Dictionary<string, List<ModelData>>();

		protected override Task OnInitializedAsync()
		{
			List<PessoaModel> pessoas = new ControladorPessoa().ObterListaPessoas();

			if (pessoas?.Any() == true)
			{
				foreach (var pessoa in pessoas)
				{
					DadosCadastraisModel modelDadosCadastrais = Utilitarios.ConverterPara<DadosCadastraisModel>(pessoa);
					DadosSocioDemograficosModel modelDadosSocioDemograficos = Utilitarios.ConverterPara<DadosSocioDemograficosModel>(pessoa);
					DadosCondicoesSaudeModel modelDadosCondicoesSaude = Utilitarios.ConverterPara<DadosCondicoesSaudeModel>(pessoa);
					DadosFamiliaModel modelDadosFamilia = Utilitarios.ConverterPara<DadosFamiliaModel>(pessoa);

					ObterValor(ValoresSexo, "Sexo", modelDadosCadastrais);
					ObterValorIdade(modelDadosCadastrais);
					ObterValor(ValoresFumante, "Fumante", modelDadosCondicoesSaude);
					ObterValor(ValoresTeveCOVID19, "TeveCOVID19", modelDadosCondicoesSaude);
					ObterValor(ValoresResponsavelFamilia, "ResponsavelFamilia", modelDadosFamilia);
					ObterValor(ValoresCodSituacaoMercado, "CodSituacaoMercado", modelDadosSocioDemograficos);
				}

				ListaIndicadores["Sexo"] = ValoresSexo;
				ListaIndicadores["Idade"] = ValoresIdade;
				ListaIndicadores["Fumante"] = ValoresFumante;
				ListaIndicadores["Responsável Familiar"] = ValoresResponsavelFamilia;
				ListaIndicadores["Teve COVID19"] = ValoresTeveCOVID19;
				ListaIndicadores["Situação no Mercado de Trabalho"] = ValoresCodSituacaoMercado;
			}

			return base.OnInitializedAsync();
		}

		private string ObterValorAtributo(object o)
		{
			string valor = "Sem informação";

			if (o != null && !string.IsNullOrWhiteSpace(o.ToString()))
			{
				valor = o.ToString();
			}

			return valor;
		}

		private void ObterValor(List<ModelData> valoresADefinir, string atributo, object model)
		{
			Dictionary<string, string> resumo = new Dictionary<string, string>();
			resumo.ObterResumo(model, new List<string>() { atributo });

			var keyValuePair = resumo.First();

			string valorAtributo = ObterValorAtributo(keyValuePair.Value);

			ModelData valor = valoresADefinir.FirstOrDefault(s => s.ValorAtributo == valorAtributo);

			if (valor == null)
			{
				valoresADefinir.Add(
					new ModelData()
					{
						ValorAtributo = valorAtributo,
						Total = 1
					});
			}
			else
			{
				valor.Total++;
			}
		}

		private void ObterValorIdade(DadosCadastraisModel modelDadosCadastrais)
		{
			if (modelDadosCadastrais.Idade.HasValue)
			{
				string faixaEtaria = string.Empty;
				if (modelDadosCadastrais.Idade < 10)
				{
					faixaEtaria = "0 a 9";
				}
				else if (modelDadosCadastrais.Idade < 20)
				{
					faixaEtaria = "10 a 19";
				}
				else if (modelDadosCadastrais.Idade < 30)
				{
					faixaEtaria = "20 a 29";
				}
				else if (modelDadosCadastrais.Idade < 40)
				{
					faixaEtaria = "30 a 39";
				}
				else if (modelDadosCadastrais.Idade < 50)
				{
					faixaEtaria = "40 a 49";
				}
				else if (modelDadosCadastrais.Idade < 60)
				{
					faixaEtaria = "50 a 59";
				}
				else
				{
					faixaEtaria = "60 e +";
				}

				ModelData valor = ValoresIdade.FirstOrDefault(s => s.ValorAtributo == faixaEtaria);

				if (valor == null)
				{
					ValoresIdade.Add(
						new ModelData()
						{
							ValorAtributo = faixaEtaria,
							Total = 1
						});
				}
				else
				{
					valor.Total++;
				}
			}

		}
	}



	public class ModelData
	{
		public string ValorAtributo { get; set; }
		public double Percentual { get; set; }
		public long Total { get; set; }
		public bool Explode { get; set; }
	}
}
