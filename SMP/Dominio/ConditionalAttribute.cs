using System.ComponentModel.DataAnnotations;

namespace SMP.Dominio
{
	public class ConditionalAttribute : ValidationAttribute
	{
		public string Message { get; set; }
		public string Converter { get; set; }

		public override bool IsValid(object? value)
		{
			return true;
		}
	}
}
