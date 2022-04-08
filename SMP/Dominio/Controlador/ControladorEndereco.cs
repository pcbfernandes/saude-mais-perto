using Newtonsoft.Json;
using SMP.Dominio.Model;

namespace SMP.Dominio.Controlador
{
	public class ControladorEndereco : ControladorBase
	{
		public List<PaisModel> ObterPaises()
		{
			var listaESUS = new ControladorDadosEsus().ObterOpcoesPais();
			List<PaisModel> retorno = (from l in listaESUS
									   select new PaisModel()
									   {
										   CodPais = l.Value,
										   Descricao = l.Key
									   }).ToList();

			return retorno;
		}
		public Dictionary<string, long> ObterOpcoesEstado()
		{
			Dictionary<string, long> retorno = new Dictionary<string, long>();

			foreach (var item in ObterEstados())
			{
				retorno[item.Descricao] = (long)item.CodEstado;
			}

			return retorno;
		}
		public List<EstadoModel> ObterEstados()
		{
			var lista = new List<EstadoModel>()
			{
				new EstadoModel() { Descricao = "ACRE", Sigla = "AC", CodEstado = 1},
				new EstadoModel() { Descricao = "ALAGOAS", Sigla = "AL", CodEstado = 2},
				new EstadoModel() { Descricao = "AMAPÁ", Sigla = "AP", CodEstado = 3},
				new EstadoModel() { Descricao = "AMAZONAS", Sigla = "AM", CodEstado = 4},
				new EstadoModel() { Descricao = "BAHIA", Sigla = "BA", CodEstado = 5},
				new EstadoModel() { Descricao = "CEARÁ", Sigla = "CE", CodEstado = 6},
				new EstadoModel() { Descricao = "DISTRITO FEDERAL", Sigla = "DF", CodEstado = 7},
				new EstadoModel() { Descricao = "ESPÍRITO SANTO", Sigla = "ES", CodEstado = 8},
				new EstadoModel() { Descricao = "GOIÁS", Sigla = "GO", CodEstado = 10},
				new EstadoModel() { Descricao = "MARANHÃO", Sigla = "MA", CodEstado = 11},
				new EstadoModel() { Descricao = "MATO GROSSO", Sigla = "MT", CodEstado = 12},
				new EstadoModel() { Descricao = "MATO GROSSO DO SUL", Sigla = "MS", CodEstado = 13},
				new EstadoModel() { Descricao = "MINAS GERAIS", Sigla = "MG", CodEstado = 14},
				new EstadoModel() { Descricao = "PARÁ", Sigla = "PA", CodEstado = 15},
				new EstadoModel() { Descricao = "PARAÍBA", Sigla = "PB", CodEstado = 16},
				new EstadoModel() { Descricao = "PARANÁ", Sigla = "PR", CodEstado = 17},
				new EstadoModel() { Descricao = "PERNAMBUCO", Sigla = "PE", CodEstado = 18},
				new EstadoModel() { Descricao = "PIAUÍ", Sigla = "PI", CodEstado = 19},
				new EstadoModel() { Descricao = "RIO DE JANEIRO", Sigla = "RJ", CodEstado = 20},
				new EstadoModel() { Descricao = "RIO GRANDE DO NORTE", Sigla = "RN", CodEstado = 21},
				new EstadoModel() { Descricao = "RIO GRANDE DO SUL", Sigla = "RS", CodEstado = 22},
				new EstadoModel() { Descricao = "RONDÔNIA", Sigla = "RO", CodEstado = 23},
				new EstadoModel() { Descricao = "RORAIMA", Sigla = "RR", CodEstado = 9},
				new EstadoModel() { Descricao = "SANTA CATARINA", Sigla = "SC", CodEstado = 25},
				new EstadoModel() { Descricao = "SÃO PAULO", Sigla = "SP", CodEstado = 26},
				new EstadoModel() { Descricao = "SERGIPE", Sigla = "SE", CodEstado = 27},
				new EstadoModel() { Descricao = "TOCANTINS", Sigla = "TO", CodEstado = 24},
			};

			return lista;
		}

		public List<MunicipioModel> ObterMunicipios(long? codEstado)
		{
			return ObterMunicipios(null, codEstado);
		}
		public List<MunicipioModel> ObterMunicipios(string? siglaEstado)
		{
			return ObterMunicipios(siglaEstado, null);
		}
		private List<MunicipioModel> ObterMunicipios(string? siglaEstado, long? codEstado)
		{
			if (string.IsNullOrWhiteSpace(siglaEstado))
			{
				var estado = ObterEstados().FirstOrDefault(e => e.CodEstado == codEstado);
				siglaEstado = estado?.Sigla;
			}

			return ObterTodosMunicipios().Where(m => m.UF == siglaEstado).ToList();
		}
		public string ObterNomeMunicipio(string codIbge)
		{
			MunicipioModel municipio = ObterTodosMunicipios().FirstOrDefault(m => m.CodigoIBGE == codIbge);
			return municipio?.Nome;
		}
		private List<MunicipioModel> ObterTodosMunicipios()
		{
			IEnumerable<MunicipioModel> dbMunicipios = _context.DbMunicipios.FindAll();

			return dbMunicipios.ToList();
		}

		public List<BairroModel> ObterBairros(string? codMunicipioIbge)
		{
			var lista = new List<BairroModel>()
			{
				new BairroModel(){ CodBairro = 1, Nome = "Bento Ferreira", CodMunicipioIbge = "3205309" },
				new BairroModel(){ CodBairro = 2, Nome = "Centro", CodMunicipioIbge = "3205309" },
			};

			return lista.Where(l => l.CodMunicipioIbge == codMunicipioIbge).ToList();
		}

		public async Task<CepModel> ObterEnderecoCep(string cep)
		{
			CepModel retorno = new CepModel();

			if (!AppConfig.DesabilitarBuscaCep)
			{
				try
				{
					var clientHandler = new HttpClientHandler
					{
						UseCookies = false,
					};

					var client = new HttpClient(clientHandler);
					var request = new HttpRequestMessage
					{
						Method = HttpMethod.Get,
						RequestUri = new Uri(string.Concat("https://labiaps-api.azurewebsites.net/Cep/", cep)),
						Headers =
					{
						//{ "cookie", "ARRAffinity=22a7daa836b64a8ce56c907737553d08297ff2e76cd06a1f52c29956b9a85c17; ARRAffinitySameSite=22a7daa836b64a8ce56c907737553d08297ff2e76cd06a1f52c29956b9a85c17" },
						{ "Authorization", $"Basic {AppConfig.CredenciaisApi}" },
					},
					};

					using (var response = await client.SendAsync(request))
					{
						response.EnsureSuccessStatusCode();
						var body = await response.Content.ReadAsStringAsync();

						if (!string.IsNullOrWhiteSpace(body))
						{
							retorno = JsonConvert.DeserializeObject<CepModel>(body);

							if (retorno != null)
							{
								retorno.Estado = ObterEstados().FirstOrDefault(e => e.Sigla == retorno.SiglaEstado);
								retorno.Municipio = ObterMunicipios(retorno.SiglaEstado).FirstOrDefault(m => m.CodigoIBGE == retorno.CodMunicipioIbge);
							}
						}
					}
				}
				catch (Exception ex)
				{
					retorno.MensagemErro = ex.Message;
				}
			}

			return retorno;
		}
	}
}
