using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using SMP.Dominio;
using SMP.Dominio.Model;
using SMP.Dominio.Controlador;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.FileSelect;

namespace SMP.Pages
{
	public class FileUploadBase : ComponentBase
	{
		[Inject] private NavigationManager _navigationManager { get; set; }
		[Inject] private IWebHostEnvironment Environment { get; set; }
		[Inject] private ILogger<FileUploadBase> Logger { get; set; }
		public List<IBrowserFile> loadedFiles = new();
		public long maxFileSize = 1024 * 1024 * 30;
		public int maxAllowedFiles = 1;
		public bool isLoading;

		public ArquivoModel ModelArquivo { get; set; }
		public ResultadoExtracaoCpfModel ResultadoProcessamento { get; set; }		
		public KeyValuePair<string, bool?> LogInformativoUpload { get; set; }
		public KeyValuePair<string, bool?> LogInformativoProcessamento { get; set; }

		public string FileName { get; set; }
		public async Task LoadFiles(InputFileChangeEventArgs e)
		{
			isLoading = true;
			loadedFiles.Clear();

			LogInformativoUpload = new KeyValuePair<string, bool?>();
			LogInformativoProcessamento = new KeyValuePair<string, bool?>();
			FileName = string.Empty;


			AtualizarLog("Upload", "Carregando arquivo...", null);			

			foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
			{
				try
				{
					loadedFiles.Add(file);
					FileName = file.Name;

					string tempPath = Utilitarios.ObterPastaTemporaria();
					string unsafeUploadsPath = Path.Combine(tempPath, "unsafe_uploads");

					if (!(Directory.Exists(unsafeUploadsPath)))
					{
						Directory.CreateDirectory(unsafeUploadsPath);
					}

					var trustedFileNameForFileStorage = Path.GetRandomFileName() + file.Name.Substring(file.Name.LastIndexOf('.'));
					var path = Path.Combine(unsafeUploadsPath, trustedFileNameForFileStorage);

					if (System.Diagnostics.Debugger.IsAttached)
					{
						await Task.Delay(3000);
					}


					byte[] bytes;

					using (FileStream fs = new(path, FileMode.Create))
					{
						await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

						AtualizarLog("Upload", "Arquivo carregado", true);

						AtualizarLog("Processamento", "Processando arquivo...", null);

						bytes = new byte[fs.Length];
						fs.Write(bytes, 0, bytes.Length);
					}

					

				}
				catch (Exception ex)
				{
					Logger.LogError("File: {Filename} Error: {Error}",
				   file.Name, ex.Message);
				}
			}

			isLoading = false;
		}

		public void SelecionarCPF(string cpf)
		{
			ModelArquivo.PessoaCPF = cpf;
			ControladorArquivo controladorArquivo = new ControladorArquivo();
			controladorArquivo.InserirArquivo(ModelArquivo);

			_navigationManager.NavigateTo($"cadastro/{cpf}");
		}

		public void AtualizarLog(string identificador, string mensagem, bool? concluido)
		{
			if(identificador == "Upload")
			{
				LogInformativoUpload = new KeyValuePair<string, bool?>(mensagem, concluido);				
			}
			else
			{
				LogInformativoProcessamento = new KeyValuePair<string, bool?>(mensagem, concluido);
			}

			StateHasChanged();
		}

		public Dictionary<string, CancellationTokenSource> Tokens { get; set; } = new Dictionary<string, CancellationTokenSource>();
		public async Task HandleFiles(FileSelectEventArgs args)
		{
			ResultadoProcessamento = null;

			foreach (var file in args.Files)
			{
				if (!file.InvalidExtension)
				{
					isLoading = true;
					loadedFiles.Clear();

					LogInformativoUpload = new KeyValuePair<string, bool?>();
					LogInformativoProcessamento = new KeyValuePair<string, bool?>();
					FileName = string.Empty;

					AtualizarLog("Upload", "Carregando arquivo...", null);

					await UploadFile(file);
				}
			}
		}

		private async Task UploadFile(FileSelectFileInfo file)
		{
			try
			{
				string tempPath = Utilitarios.ObterPastaTemporaria();
				string unsafeUploadsPath = Path.Combine(tempPath, "unsafe_uploads");
				if (!(Directory.Exists(unsafeUploadsPath)))
				{
					Directory.CreateDirectory(unsafeUploadsPath);
				}

				Tokens.Add(file.Id, new CancellationTokenSource());
				var path = Path.Combine(unsafeUploadsPath, file.Name);

				byte[] bytes;
				byte[] buffer = new byte[16 * 1024];
				using (MemoryStream ms = new MemoryStream())
				{
					int read;
					while ((read = await file.Stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
					{
						ms.Write(buffer, 0, read);
					}
					bytes = ms.ToArray();

					AtualizarLog("Upload", "Arquivo carregado", true);
				}

				AtualizarLog("Processamento", "Processando arquivo...", null);

				ControladorArquivo controladorArquivo = new ControladorArquivo();

				ModelArquivo = new ArquivoModel()
				{
					Dados = new ArquivoDadosModel() { Dados = bytes },
					Nome = file.Name,
					Tamanho = file.Size,
					//Tipo = file.ContentType,
				};

				string base64 = Convert.ToBase64String(bytes);

				ResultadoProcessamento = await controladorArquivo.ExtrairCPF(base64);

				if (ResultadoProcessamento?.Sucesso == true)
				{
					AtualizarLog("Processamento", "Processamento concluído", true);
				}
				else
				{
					AtualizarLog("Processamento", $"Erro ao processar o arquivo: {ResultadoProcessamento.MensagemErro}", false);
				}

			}
			catch (Exception ex)
			{

			}
			finally
			{
				isLoading = false;
			}
		}

		public async Task HandleRemoveFiles(FileSelectEventArgs args)
		{
			LogInformativoUpload = new KeyValuePair<string, bool?>();
			LogInformativoProcessamento = new KeyValuePair<string, bool?>();
			FileName = string.Empty;
			ResultadoProcessamento = null;
		}
	}

	public class CustomLog
	{
		public string Identificador { get; set; }
		public string Mensagem { get; set; }
		public bool Concluido { get; set; }
	}
}
