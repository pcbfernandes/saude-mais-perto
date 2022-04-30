using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{

	public class ResultadoValidacaoPessoaModel
	{
		public List<ValidacaoPessoaModel> ListaValidacaoPessoa { get; set; }
		public bool? EstaValido { get; set; }
		public string ResultadoValidacao { get; set; }
	}
	public class ValidacaoPessoaModel
	{
		public int Id { get; set; }
		public string? CPF { get; set; }

		[Display(Name = "O cadastro está válido?")]
		public bool PessoaValidada { get; set; }

		[Display(Name = "Mensagem")]
		[Required(ErrorMessage = "A mensagem deve ser informada.")]
		public string? Mensagem { get; set; }
		public DateTime? DataCriacao { get; set; }
		public DateTime? DataVisualizacao { get; set; }
		public DateTime? DataExclusao { get; set; }
	}
}
