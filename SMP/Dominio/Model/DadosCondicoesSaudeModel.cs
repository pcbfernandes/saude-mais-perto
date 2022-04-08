using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class DadosCondicoesSaudeModel
	{
		[Display(Name = "Gestante?")]
		[ESUS(Nome = "statusEhGestante")]
		public bool Gestante { get; set; }

		[Display(Name = "Em relação ao seu peso, você se considera com?")]
		[ESUS(Nome = "situacaoPeso")]
		[Conditional(Converter = "ObterOpcoesConsideracaoPeso")]
		public long? CodConsideracaoPeso { get; set; }

		[Display(Name = "Fumante?")]
		[ESUS(Nome = "statusEhFumante")]
		public bool Fumante { get; set; }

		[Display(Name = "Faz uso de álcool?")]
		[ESUS(Nome = "statusEhDependenteAlcool")]
		public bool FazUsoAlcool { get; set; }

		[Display(Name = "Faz uso de outras drogas?")]
		[ESUS(Nome = "statusEhDependenteOutrasDrogas")]
		public bool FazUsoOutrasDrogas { get; set; }

		[Display(Name = "Tem hipertensao arterial?")]
		[ESUS(Nome = "statusTemHipertensaoArterial")]
		public bool TemHipertensaoArterial { get; set; }

		[Display(Name = "Tem diabetes?")]
		[ESUS(Nome = "statusTemDiabetes")]
		public bool TemDiabetes { get; set; }

		[Display(Name = "Teve AVC / derrame?")]
		[ESUS(Nome = "statusTeveAvcDerrame")]
		public bool TeveAvcDerrame { get; set; }

		[Display(Name = "Teve infarto?")]
		[ESUS(Nome = "statusTeveInfarto")]
		public bool TeveInfarto { get; set; }

		[Display(Name = "Teve doença cardiaca?")]
		[ESUS(Nome = "statusTeveDoencaCardiaca")]
		public bool TeveDoencaCardiaca { get; set; }

		[Display(Name = "Tem doença respiratória?")]
		[ESUS(Nome = "statusTemDoencaRespiratoria")]
		public bool TemDoencaRespiratoria { get; set; }

		[Display(Name = "Está com hanseníase?")]
		[ESUS(Nome = "statusTemHanseniase")]
		public bool TemHanseniase { get; set; }

		[Display(Name = "Está com tuberculose?")]
		[ESUS(Nome = "statusTemTuberculose")]
		public bool TemTuberculose { get; set; }

		[Display(Name = "Tem ou teve câncer?")]
		[ESUS(Nome = "statusTemTeveCancer")]
		public bool TemTeveCancer { get; set; }

		[Display(Name = "Tem diagnóstico de problema de saúde mental por profissional de saúde?")]
		[ESUS(Nome = "statusDiagnosticoMental")]
		public bool DiagnosticoMental { get; set; }

		[Display(Name = "Está acamado?")]
		[ESUS(Nome = "statusEstaAcamado")]
		public bool Acamado { get; set; }

		[Display(Name = "Teve COVID-19?")]
		public bool TeveCOVID19 { get; set; }

		[Display(Name = "Situação vacinal para COVID-19:")]
		[Conditional(Converter = "ObterOpcoesSituacaoVacinalCOVID19")]
		public long? SituacaoVacinalCOVID19 { get; set; }

		[Display(Name = "Indicar outra condição de saúde?")]
		public bool PossuiOutraCondicaoSaude { get; set; }

		[Display(Name = "Qual outra condição de saúde?")]
		[Conditional(Message = "Deve ser informada qual outra condição de saúde.")]
		public string? QualOutraCondicaoSaude { get; set; }

		[Display(Name = "Está em situação de rua?")]
		[ESUS(Nome = "EmSituacaoDeRua")]
		public bool EmSituacaoDeRua { get; set; }
		[Display(Name = "Possui alguma deficiência?")]
		[ESUS(Nome = "statusTemAlgumaDeficiencia")]
		public bool PossuiDeficiencia { get; set; }

		private string? _qualDeficiencia;

		[Display(Name = "Qual deficiência?")]
		[Conditional(Message = "Deve ser selecionada alguma deficiência", Converter = "ObterDescricaoQualDeficiencia")]
		public string? QualDeficiencia { get { return _qualDeficiencia; } set { _qualDeficiencia = value; } }

		[NotMapped]
		[ESUS(Nome = "deficienciasCidadao")]
		public List<long> ListaQualDeficiencia
		{
			get { return Utilitarios.ConvertToList<long>(_qualDeficiencia); }
			set { _qualDeficiencia = Utilitarios.ConvertFromList(value); }
		}
	}
}
