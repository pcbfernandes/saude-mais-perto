using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class PessoaModel
	{
		public long Id { get; set; }
		public string? GuidPessoa { get; set; }
		public string? Nome { get; set; }
		public string? NomeSocial { get; set; }
		public long? Sexo { get; set; }
		public string? CPF { get; set; }
		public DateTime? DataNascimento { get; set; }
		public string? NomeMae { get; set; }
		public string? NomePai { get; set; }
		public string? TelefoneContato { get; set; }
		public string? CEP { get; set; }
		public long? CodEstado { get; set; }
		public string? CodMunicipioIbge { get; set; }
		public string? Bairro { get; set; }
		public string? Logradouro { get; set; }
		public string? Complemento { get; set; }
		public long? CodRacaCor { get; set; }
		public long? CodEtnia { get; set; }
		public long? CodNacionalidade { get; set; }
		public long? CodPaisNascimento { get; set; }
		public long? CodEstadoNascimento { get; set; }
		public string? CodIbgeMunicipioNascimento { get; set; }
		public DateTime? DataEtradaBrasil { get; set; }
		public string? PortariaNaturalizacao { get; set; }
		public bool ResponsavelFamilia { get; set; }
		public bool DesejaInformarResponsavelFamilia { get; set; }
		public string? CpfResponsavel { get; set; }

		public string? CnesReferencia { get; set; }

		public int? SituacaoCadastral { get; set; }



		public long? CodRelacaoParentescoResponsavel { get; set; }
		public string? CBO { get; set; }
		public long? CodGrauInstrucao { get; set; }
		public long? CodSituacaoMercado { get; set; }
		public long? CodResponsavelPorCrianca { get; set; }
		public bool FrequentaEscolaCreche { get; set; }
		public bool PossuiPlanoSaudePrivado { get; set; }
		public bool PossuiDeficiencia { get; set; }
		public string? QualDeficiencia { get; set; }



		public bool Gestante { get; set; }
		public long? CodConsideracaoPeso { get; set; }
		public bool Fumante { get; set; }
		public bool FazUsoAlcool { get; set; }
		public bool FazUsoOutrasDrogas { get; set; }
		public bool TemHipertensaoArterial { get; set; }
		public bool TemDiabetes { get; set; }
		public bool TeveAvcDerrame { get; set; }
		public bool TeveInfarto { get; set; }
		public bool TeveDoencaCardiaca { get; set; }
		public bool TemDoencaRespiratoria { get; set; }
		public bool TemHanseniase { get; set; }
		public bool TemTuberculose { get; set; }
		public bool TemTeveCancer { get; set; }
		public bool DiagnosticoMental { get; set; }
		public bool Acamado { get; set; }
		public bool TeveCOVID19 { get; set; }
		public long? SituacaoVacinalCOVID19 { get; set; }
		public bool PossuiOutraCondicaoSaude { get; set; }
		public string? QualOutraCondicaoSaude { get; set; }
		public bool EmSituacaoDeRua { get; set; }

		public DateTime? DataCriacao { get; set; }
		public DateTime? DataUltimaAtualizaco { get; set; }
		[NotMapped] public ArquivoModel ModelArquivo { get; set; }
		[NotMapped] public ResultadoValidacaoPessoaModel ModelResultadoValidacaoPessoa { get; set; }

	}
}
