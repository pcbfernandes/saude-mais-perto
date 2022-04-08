using System.ComponentModel.DataAnnotations;

namespace SMP.Dominio
{
	public class ESUSAttribute : ValidationAttribute
	{
		public string Nome { get; set; }

		public override bool IsValid(object? value)
		{
			return true;
		}
	}
}
