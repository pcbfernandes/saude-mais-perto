using Newtonsoft.Json;

namespace SMP.Dominio.Model
{
	public class ConsultaCpfModel
	{
		public Dados Dados { get; set; }
		public bool Sucesso { get; set; }
		public object Error { get; set; }
	}
	public class Carto
	{
		public string numeroCNS { get; set; }
		public DateTime dataAtribuicao { get; set; }
		public bool dataAtribuicaoSpecified { get; set; }
		public int tipoCartao { get; set; }
		public bool tipoCartaoSpecified { get; set; }
		public bool manual { get; set; }
		public bool manualSpecified { get; set; }
		public string justificativaManual { get; set; }
	}

	public class CPF
	{
		public string numeroCPF { get; set; }
	}

	public class NomeCompleto
	{
		public string Nome { get; set; }
	}

	public class Mae
	{
		public string Nome { get; set; }
	}

	public class Pai
	{
		public string Nome { get; set; }
	}

	public class Sexo
	{
		public string codigoSexo { get; set; }
		public string descricaoSexo { get; set; }
	}

	public class RacaCor
	{
		public string codigoRacaCor { get; set; }
		public string descricaoRacaCor { get; set; }
	}

	public class EtniaIndigena
	{
		public string codigoEtniaIndigena { get; set; }
		public object descricaoEtniaIndigena { get; set; }
	}

	public class PaisNascimento
	{
		public string codigoPais { get; set; }
		public string codigoPaisAntigo { get; set; }
		public string nomePais { get; set; }
	}

	public class DadosNacionalidade
	{
		public int nacionalidade { get; set; }
		public PaisNascimento PaisNascimento { get; set; }
		public bool dataNaturalizacaoSpecified { get; set; }
		public string numeroPortariaNaturalizacao { get; set; }
		public bool dataChegadaBrasilSpecified { get; set; }
	}

	public class UF
	{
		public string codigoUF { get; set; }
		public string siglaUF { get; set; }
		public string codigoRegiao { get; set; }
		public string nomeUF { get; set; }
	}

	public class MunicipioNascimento
	{
		public string codigoMunicipio { get; set; }
		public string nomeMunicipio { get; set; }
		public UF UF { get; set; }
	}

	public class OrgaoEmissor
	{
		public string codigoOrgaoEmissor { get; set; }
		public string nomeOrgaoEmissor { get; set; }
		public string siglaOrgaoEmissor { get; set; }
	}

	public class Identidade
	{
		public string identificador { get; set; }
		public string numeroIdentidade { get; set; }
		public DateTime dataExpedicao { get; set; }
		public bool dataExpedicaoSpecified { get; set; }
		public OrgaoEmissor OrgaoEmissor { get; set; }
		public string siglaUF { get; set; }
	}

	public class Documentos
	{
		public Identidade Identidade { get; set; }
		public object CTPS { get; set; }
		public object CNH { get; set; }
		public object TituloEleitor { get; set; }
		public object NIS { get; set; }
		public object Passaporte { get; set; }
		public object RIC { get; set; }
		public object DNV { get; set; }
	}

	public class TipoLogradouro
	{
		public string codigoTipoLogradouro { get; set; }
		public string descricaoTipoLogradouro { get; set; }
	}

	public class Bairro
	{
		public string codigoBairro { get; set; }
		public string descricaoBairro { get; set; }
	}

	public class CEP
	{
		public string numeroCEP { get; set; }
	}

	public class Municipio
	{
		public string codigoMunicipio { get; set; }
		public string nomeMunicipio { get; set; }
		public UF UF { get; set; }
	}

	public class Pais
	{
		public string codigoPais { get; set; }
		public string codigoPaisAntigo { get; set; }
		public string nomePais { get; set; }
	}

	public class Endereco
	{
		public string identificador { get; set; }
		public int TipoEndereco { get; set; }
		public bool TipoEnderecoSpecified { get; set; }
		public TipoLogradouro TipoLogradouro { get; set; }
		public string nomeLogradouro { get; set; }
		public string numero { get; set; }
		public string complemento { get; set; }
		public Bairro Bairro { get; set; }
		public CEP CEP { get; set; }
		public Municipio Municipio { get; set; }
		public Pais Pais { get; set; }
		public string municipioInternacional { get; set; }
	}

	public class Enderecos
	{
		public Endereco Endereco { get; set; }
	}

	public class TipoTelefone
	{
		public string codigoTipoTelefone { get; set; }
		public string descricaoTipoTelefone { get; set; }
	}

	public class Telefone
	{
		public string identificador { get; set; }
		public TipoTelefone TipoTelefone { get; set; }
		public string DDI { get; set; }
		public string DDD { get; set; }
		public string numeroTelefone { get; set; }
	}

	public class IdentificadorCorporativo
	{
		public string numeroIdentificadorCorporativo { get; set; }
	}

	public class GrauQualidade
	{
		public string percentualQualidade { get; set; }
	}

	public class Dados
	{
		public IList<Carto> Cartoes { get; set; }
		public CPF CPF { get; set; }
		public NomeCompleto NomeCompleto { get; set; }
		public string NomeSocial { get; set; }
		public DateTime dataNascimento { get; set; }
		public Mae Mae { get; set; }
		public Pai Pai { get; set; }
		public bool dataObitoSpecified { get; set; }
		public Sexo Sexo { get; set; }
		public RacaCor RacaCor { get; set; }
		public EtniaIndigena EtniaIndigena { get; set; }
		public string TipoSanguineo { get; set; }
		public DadosNacionalidade DadosNacionalidade { get; set; }
		public MunicipioNascimento MunicipioNascimento { get; set; }
		public Documentos Documentos { get; set; }
		public object Certidoes { get; set; }
		public Enderecos Enderecos { get; set; }
		public IList<Telefone> Telefones { get; set; }
		public object Emails { get; set; }
		public object Fotografias { get; set; }
		public object IdentificadorLocal { get; set; }
		public IdentificadorCorporativo IdentificadorCorporativo { get; set; }
		public GrauQualidade GrauQualidade { get; set; }
		public bool originalRFB { get; set; }
		public bool originalRFBSpecified { get; set; }
		public bool nomade { get; set; }
		public bool nomadeSpecified { get; set; }
		public object DadosPreCadastro { get; set; }
		public bool Situacao { get; set; }
		public bool SituacaoSpecified { get; set; }
		public string motivoAlteracaoSituacao { get; set; }
		public bool Vip { get; set; }
		public bool VipSpecified { get; set; }
		public string motivoAlteracaoVip { get; set; }
		public bool protecaoTestemunha { get; set; }
		public bool protecaoTestemunhaSpecified { get; set; }
		public string descricaoProtecaoTestemunha { get; set; }
		public string motivoNaoHigienizado { get; set; }
		public bool vivo { get; set; }
		public bool vivoSpecified { get; set; }
	}
}

