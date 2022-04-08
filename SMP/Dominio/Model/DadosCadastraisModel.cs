using System;
using System.ComponentModel.DataAnnotations;

namespace SMP.Dominio.Model
{
	public class DadosCadastraisModel
	{
		[Display(Name = "Nome:")]
		[ESUS(Nome = "nomeCidadao")]
		[Required(ErrorMessage = "O nome deve ser informado.")]
		public string? Nome { get; set; }

		[Display(Name = "Nome Social:")]
		[ESUS(Nome = "nomeSocial")]
		public string? NomeSocial { get; set; }

		[Display(Name = "Sexo:")]
		[ESUS(Nome = "sexoCidadao")]
		[Required(ErrorMessage = "O sexo deve ser informado.")]
		[Conditional (Converter = "ObterOpcoesSexo")]
		public long? Sexo { get; set; }

		[Display(Name = "CPF:")]
		[Required(ErrorMessage = "O CPF deve ser informado.")]
		public string? CPF { get; set; }

		[Display(Name = "Nascimento:")]
		[ESUS(Nome = "dataNascimentoCidadao")]
		[Required(ErrorMessage = "A data de nascimento deve ser informada.")]
		public DateTime? DataNascimento { get; set; }
		public int? Idade { get { return DataNascimento.HasValue ? (int)((DateTime.Today - DataNascimento.Value.Date).TotalDays / 365) : null; } }

		[Display(Name = "Nome Mãe:")]
		[Required(ErrorMessage = "O nome da mãe deve ser informado.")]
		[ESUS(Nome = "nomeMaeCidadao")]
		public string? NomeMae { get; set; }

		[Display(Name = "Nome Pai:")]
		[ESUS(Nome = "nomePaiCidadao")]
		public string? NomePai { get; set; }

		[Display(Name = "Telefone de Contato:")]
		public string? TelefoneContato { get; set; }

		private string _cep;

		[Display(Name = "CEP:")]
		[Required(ErrorMessage = "O CEP deve ser informado.")]
		public string? CEP { get { return _cep != null ? _cep.Trim() : _cep; } set { _cep = value; } }

		[Display(Name = "Estado:")]
		[Required(ErrorMessage = "O estado deve ser informado.")]
		[Conditional(Converter = "ObterOpcoesEstado")]
		public long? CodEstado { get; set; }

		[Display(Name = "Município:")]
		[Required(ErrorMessage = "O município deve ser informado.")]
		[Conditional(Converter = "ObterNomeMunicipio")]
		public string? CodMunicipioIbge { get; set; }

		[Display(Name = "Bairro:")]
		[Required(ErrorMessage = "O bairro deve ser informado.")]
		public string? Bairro { get; set; }

		[Display(Name = "Logradouro:")]
		[Required(ErrorMessage = "O logradouro deve ser informado.")]
		public string? Logradouro { get; set; }

		[Display(Name = "Complemento:")]
		public string? Complemento { get; set; }

		[Display(Name = "Raça/Cor:")]
		[ESUS(Nome = "racaCorCidadao")]
		[Required(ErrorMessage = "A raça/cor deve ser informada.")]
		[Conditional(Converter = "ObterOpcoesRacaCor")]
		public long? CodRacaCor { get; set; }

		[Display(Name = "Etnia:")]
		[ESUS(Nome = "etnia")]
		[Conditional(Converter = "ObterOpcoesEtnia")]
		//[Required(ErrorMessage = "A etnia deve ser informada.")]
		public long? CodEtnia { get; set; }

		[Display(Name = "Nacionalidade:")]
		[ESUS(Nome = "nacionalidadeCidadao")]
		[Required(ErrorMessage = "A nacionalidade deve ser informada.")]
		[Conditional(Converter = "ObterOpcoesNacionalidade")]
		public long? CodNacionalidade { get; set; }

		[Display(Name = "País Nascimento:")]
		[ESUS(Nome = "paisNascimento")]
		[Conditional(Message = "O país de nascimento deve ser informado.", Converter = "ObterOpcoesPais")]
		public long? CodPaisNascimento { get; set; }

		[Display(Name = "Estado Nascimento:")]
		[Conditional(Message = "O estado de nascimento deve ser informado.", Converter = "ObterOpcoesEstado")]
		public long? CodEstadoNascimento { get; set; }

		[Display(Name = "Município Nascimento:")]
		[ESUS(Nome = "codigoIbgeMunicipioNascimento")]
		[Conditional(Message = "O município de nascimento deve ser informado.", Converter = "ObterNomeMunicipio")]
		public string? CodIbgeMunicipioNascimento { get; set; }

		[Display(Name = "Data de Entrada no Brasil:")]
		[ESUS(Nome = "dtEntradaBrasil")]
		[Conditional(Message = "A data de entrrada no Brasil deve ser informada.")]
		public DateTime? DataEtradaBrasil { get; set; }

		[Display(Name = "Portaria de Naturalização:")]
		[ESUS(Nome = "portariaNaturalizacao")]
		[StringLength(16)]
		[Conditional(Message = "A portaria de naturalização deve ser informada.")]
		public string? PortariaNaturalizacao { get; set; }

		[Display(Name = "Data de Naturalização:")]
		[ESUS(Nome = "dtNaturalizacao")]
		[Conditional(Message = "A data de naturalização deve ser informada.")]
		public DateTime? DataNaturalizacao { get; set; }
		
		public int? SituacaoCadastral { get; set; }
		
		[Display(Name = "US de Referência:")]
		public string? DescricaoCnesReferencia { get; set; }

		[Display(Name = "US de Referência:")]
		[Conditional(Converter = "ObterDescricoUnidadeSaude")]
		public string? CnesReferencia { get; set; }
	}
}
