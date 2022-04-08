#nullable disable
using Newtonsoft.Json;
using SMP.Dominio.Model;
using System.Net.Http.Headers;

namespace SMP.Dominio.Controlador
{
	public class ControladorArquivo : ControladorBase
	{
		public ArquivoModel ObterArquivoModel(string cpf, bool incluirDados)
		{
			ArquivoModel arquivo = _context.DbArquivo.FindOne(a => a.PessoaCPF == cpf && !a.DataExclusao.HasValue);

			if (incluirDados && arquivo != null)
			{
				arquivo.Dados = ObterArquivoDadosModel(arquivo.IdArquivoDados.Value);
			}

			return arquivo;
		}
		public ArquivoDadosModel ObterArquivoDadosModel(long idArquivoDados)
		{
			return _context.DbArquivoDados.FindOne(d => d.Id == idArquivoDados);
		}
		public long InserirDados(byte[] byteArray)
		{
			ArquivoDadosModel dados = new ArquivoDadosModel() { Dados = byteArray };
			var bson = _context.DbArquivoDados.Insert(dados);

			return dados.Id;
		}
		public void InserirArquivo(ArquivoModel arquivo)
		{
			long idArquivoDados = InserirDados(arquivo.Dados.Dados);
			arquivo.IdArquivoDados = idArquivoDados;

			_context.DbArquivo.Insert(arquivo);
		}

		public async Task<ResultadoExtracaoCpfModel> ExtrairCPF(string base64)
		{
			ResultadoExtracaoCpfModel retorno = new ResultadoExtracaoCpfModel();

			if (!AppConfig.DesabilitarProcessamentoArquivo)
			{

				try
				{
					var bytes = Convert.FromBase64String(base64);

					var content = new MultipartFormDataContent();
					var file_content = new ByteArrayContent(bytes);
					file_content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
					file_content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
					{
						FileName = "anexo.pdf",
						Name = "arquivo",
					};

					content.Add(file_content);

					var clientHandler = new HttpClientHandler { UseCookies = false, };
					var client = new HttpClient(clientHandler);
					var request = new HttpRequestMessage
					{
						Method = HttpMethod.Post,
						RequestUri = new Uri("https://labiaps-api.azurewebsites.net/Ocr/ExtractCPF"),
						Headers =
						{
							{ "Authorization", $"Basic {AppConfig.CredenciaisApi}" },
						},
						Content = content,
					};

					using (var response = await client.SendAsync(request))
					{
						response.EnsureSuccessStatusCode();
						var body = await response.Content.ReadAsStringAsync();

						if (!string.IsNullOrWhiteSpace(body))
						{
							retorno = JsonConvert.DeserializeObject<ResultadoExtracaoCpfModel>(body);
						}
					}
				}
				catch (Exception ex)
				{
					retorno.MensagemErro = ex.Message;
				}
			}
			else
			{
				if (System.Diagnostics.Debugger.IsAttached)
				{
					await Task.Delay(3000);
				}

				retorno.Sucesso = true;
				retorno.ListaCPF = new List<ResultadoExtracaoCpfListaCpfModel>()
				{
					new ResultadoExtracaoCpfListaCpfModel(){ Valor = "68895347005", ValorOriginal = "688.953.470-05" },
				};
				//retorno.MensagemErro = "Arquivo inválido";
			}

			return retorno;
		}

	}
}
