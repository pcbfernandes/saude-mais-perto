using System.ComponentModel.DataAnnotations;

namespace SMP.Dominio.Model
{
	public class AcessoModel
	{
		[Display(Name = "CPF:")]
		[Required(ErrorMessage = "O CPF deve ser informado.")]
		public string? CPF { get; set; }
	}
}
