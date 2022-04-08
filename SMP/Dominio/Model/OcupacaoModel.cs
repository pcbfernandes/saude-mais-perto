using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class OcupacaoModel
	{
		public string? CBO { get; set; }
		public string? Descricao { get; set; }
		[NotMapped]
		public string? DescricaoCBO { get { return $"{CBO}-{Descricao}"; } }
	}
}
