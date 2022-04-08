using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class DadosFamiliaModel
	{
		[Display(Name = "É Responsável Familiar:")]
		[ESUS(Nome = "statusEhResponsavel")]
		public bool ResponsavelFamilia { get; set; }

		[Display(Name = "Deseja Informar o CPF do Responsável Familiar:")]
		public bool DesejaInformarResponsavelFamilia { get; set; }


		[Display(Name = "CPF do Responsável Familiar:")]
		[Conditional(Message = "O CPF do Responsável Familiar deve ser válido!")]
		public string? CpfResponsavel { get; set; }

		[Display(Name = "Relação de parentesco com o Responsável Familiar:")]
		[ESUS(Nome = "relacaoParentescoCidadao")]
		[Conditional(Converter = "ObterOpcoesRelacaoParentesco")]
		public long? CodRelacaoParentescoResponsavel { get; set; }
	}
}
