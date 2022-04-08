using Newtonsoft.Json;

namespace SMP.Dominio.Model
{
	public class ResultadoExtracaoCpfModel
	{

		[JsonProperty("listaCPF")]
		public List<ResultadoExtracaoCpfListaCpfModel> ListaCPF { get; set; }

		[JsonProperty("sucesso")]
		public bool Sucesso { get; set; }

		[JsonProperty("error")]
		public string MensagemErro { get; set; }
	}

	public class ResultadoExtracaoCpfListaCpfModel
	{
		[JsonProperty("valor")]
		public string Valor { get; set; }

		[JsonProperty("valorOriginal")]
		public string ValorOriginal { get; set; }
	}
}
