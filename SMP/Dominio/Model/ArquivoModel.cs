using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMP.Dominio.Model
{
	public class ArquivoModel
	{
		public long Id { get; set; }
		public string? PessoaCPF { get; set; }
		public string? Nome { get; set; }
		public string? Tipo { get; set; }
		public long? Tamanho { get; set; }		
		public long? IdArquivoDados { get; set; }
		[NotMapped] public ArquivoDadosModel? Dados { get; set; }
		public DateTime? DataInclusao { get; set; }
		public DateTime? DataExclusao { get; set; }
		public string? LoginExclusao { get; set; }

		[Required(ErrorMessage = "Insira um documento no formato PDF")]
		[Range(typeof(bool), "true", "true", ErrorMessage = "Insira um documento no formato PDF")]
		[NotMapped] public bool IsArquivoValido { get; set; }
		[NotMapped] public string TamanhoFormatado
		{
			get
			{
				string descricao = string.Empty;

				if (Tamanho.HasValue)
				{
					long tamanho = Tamanho.Value / 1024;
					if (tamanho > 1024)
					{
						tamanho = tamanho / 1024;
						descricao = $"{tamanho} MB";
					}
					else
					{
						descricao = $"{tamanho} KB";
					}
				}

				return descricao;
			}
		}
	}
}
