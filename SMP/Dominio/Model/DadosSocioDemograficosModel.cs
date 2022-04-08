using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class DadosSocioDemograficosModel
	{
		[Display(Name = "Ocupação:")]
		[Conditional(Converter = "ObterOpcoesOcupacao")]
		public string? CBO { get; set; }

		[Display(Name = "Curso mais elevado que frequenta ou frequentou:")]
		[ESUS(Nome = "grauInstrucaoCidadao")]
		[Conditional(Converter = "ObterOpcoesGrauInstrucao")]
		public long? CodGrauInstrucao { get; set; }

		[Display(Name = "Situação no mercado de trabalho:")]
		[ESUS(Nome = "situacaoMercadoTrabalhoCidadao")]
		[Conditional(Converter = "ObterOpcoesSituacaoMercado")]
		public long? CodSituacaoMercado { get; set; }

		[Display(Name = "Crianças de 0 a 9 anos, ficam com:")]
		[ESUS(Nome = "responsavelPorCrianca")]
		[Conditional(Converter = "ObterOpcoesResponsavelPorCrianca")]
		public long? CodResponsavelPorCrianca { get; set; }

		[Display(Name = "Frequenta escola ou creche?")]
		[ESUS(Nome = "statusFrequentaEscola")]
		public bool FrequentaEscolaCreche { get; set; }

		[Display(Name = "Possui plano de saúde privado?")]
		[ESUS(Nome = "statusPossuiPlanoSaudePrivado")]
		public bool PossuiPlanoSaudePrivado { get; set; }
	}
}
