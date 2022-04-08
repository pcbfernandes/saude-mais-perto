using Newtonsoft.Json;

namespace SMP.Dominio.Model
{
	public class CepModel
	{
		[JsonProperty("cep")]
		public string? NumeroCep { get; set; }

		[JsonProperty("uf")]
		public string? SiglaEstado { get; set; }

		[JsonProperty("localidade")]
		public string? Localidade { get; set; }

		[JsonProperty("ibge")]
		public string? CodMunicipioIbge { get; set; }

		[JsonProperty("bairro")]
		public string? Bairro { get; set; }

		[JsonProperty("logradouro")]
		public string? Logradouro { get; set; }

		[JsonProperty("complemento")]
		public string? Complemento { get; set; }

		[JsonProperty("ddd")]
		public string? DDD { get; set; }

		[JsonProperty("sucesso")]
		public bool? IsSucesso { get; set; }

		[JsonProperty("error")]
		public string? MensagemErro { get; set; }


		public EstadoModel? Estado { get; set; }
		public MunicipioModel? Municipio { get; set; }
	}
}
