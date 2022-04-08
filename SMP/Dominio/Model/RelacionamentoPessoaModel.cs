using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class RelacionamentoPessoaModel
	{
		public string CpfResponsavel { get; set; }
		public string Cpf { get; set; }
		public string Relacao { get; set; }
		public string NomePessoa { get; set; }
	}
}
