using Microsoft.AspNetCore.Components;
using SMP.Dominio;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;
using SMP.Shared.Component;
using Telerik.Blazor.Components;

namespace SMP.Pages
{
	public class CadastroBase : ComponentBase
	{
		[Inject] private NavigationManager _navigationManager { get; set; }
		[Parameter] public string? ParametroCPF { get; set; }
		public bool ShowWizard { get; set; } = true;
		public CustomValidation? CustomValidation { get; set; }
		public int WizardStep { get; set; }
		public TelerikForm RegisterForm { get; set; }

		public PessoaModel ModelPessoa { get; set; } = new PessoaModel();
		public DadosCadastraisModel ModelDadosCadastrais { get; set; } = new DadosCadastraisModel();
		public DadosSocioDemograficosModel ModelDadosSocioDemograficos { get; set; } = new DadosSocioDemograficosModel();
		public DadosCondicoesSaudeModel ModelDadosCondicoesSaude { get; set; } = new DadosCondicoesSaudeModel();
		public DadosFamiliaModel ModelDadosFamilia { get; set; } = new DadosFamiliaModel();
		public Dictionary<string, long>? OpcoesSexo { get; set; }
		public List<EstadoModel>? OpcoesEstado { get; set; }
		public List<MunicipioModel>? OpcoesMunicipio { get; set; }
		public List<PaisModel>? OpcoesPaisNascimento { get; set; }
		public List<EstadoModel>? OpcoesEstadoNascimento { get; set; }
		public List<MunicipioModel>? OpcoesMunicipioNascimento { get; set; }
		public List<BairroModel>? OpcoesBairro { get; set; }
		public Dictionary<string, long>? OpcoesRacaCor { get; set; }
		public Dictionary<string, long>? OpcoesEtnia { get; set; }
		public Dictionary<string, long>? OpcoesNacionalidade { get; set; }

		public Dictionary<string, long>? OpcoesRelacaoParentesco { get; set; }
		public List<OcupacaoModel>? OpcoesOcupacao { get; set; } = new List<OcupacaoModel>();
		public Dictionary<string, long>? OpcoesGrauInstrucao { get; set; }
		public Dictionary<string, long>? OpcoesSituacaoMercado { get; set; }
		public Dictionary<string, long>? OpcoesResponsavelPorCrianca { get; set; }
		public List<DeficienciaModel>? OpcoesDeficiencias { get; set; } = new List<DeficienciaModel>();

		public Dictionary<string, long>? OpcoesConsideracaoPeso { get; set; }
		public Dictionary<string, long>? OpcoesSituacaoVacinalCOVID19 { get; set; }

		public bool ExibirDivEtnia { get; set; }
		public bool ExibirDivQualDeficiencia { get; set; }
		public int IdadeCorte { get; set; } = 18;

		public bool InserirNomeSocial { get; set; }
		public bool IsBusy { get; set; }

		public TelerikNotification NotificationComponent { get; set; }

		public string LoaderMessage { get; set; } = "Carregando, por favor aguarde...";
		public ResultadoValidacaoPessoaModel ModelResultadoValidacaoPessoa { get; set; }

		protected override void OnInitialized()
		{
			ControladorDadosEsus controladorDadosEsus = new ControladorDadosEsus();
			OpcoesSexo = controladorDadosEsus.ObterOpcoesSexo();
			OpcoesNacionalidade = controladorDadosEsus.ObterOpcoesNacionalidade();
			OpcoesRacaCor = controladorDadosEsus.ObterOpcoesRacaCor();
			OpcoesRelacaoParentesco = controladorDadosEsus.ObterOpcoesRelacaoParentesco();
			OpcoesGrauInstrucao = controladorDadosEsus.ObterOpcoesGrauInstrucao();
			OpcoesSituacaoMercado = controladorDadosEsus.ObterOpcoesSituacaoMercado();
			OpcoesResponsavelPorCrianca = controladorDadosEsus.ObterOpcoesResponsavelPorCrianca();
			OpcoesConsideracaoPeso = controladorDadosEsus.ObterOpcoesConsideracaoPeso();
			OpcoesSituacaoVacinalCOVID19 = controladorDadosEsus.ObterOpcoesSituacaoVacinalCOVID19();

			ControladorEndereco controladorEndereco = new ControladorEndereco();
			OpcoesEstado = controladorEndereco.ObterEstados();
			OpcoesPaisNascimento = controladorEndereco.ObterPaises();
			OpcoesEstadoNascimento = controladorEndereco.ObterEstados();

			ControladorSigtap controladorOcupacao = new ControladorSigtap();
			OpcoesOcupacao = controladorOcupacao.ObterOpcoesOcupacao();

			base.OnInitialized();
		}
		protected override async Task OnParametersSetAsync()
		{
			IsBusy = true;
			try
			{
				if (!Utilitarios.ValidarCPF(ParametroCPF))
				{
					_navigationManager.NavigateTo("/TipoCadastro");
				}

				var controladorPessoa = new ControladorPessoa();

				ModelPessoa = await controladorPessoa.ObterPessoaPeloCPF(ParametroCPF);

				if (ModelPessoa == null)
				{
					DefinirLoaderMessage("Obtendo informações cadastrais de outros serviços.");
					ControladorBuscaDadosPessoa controladorBuscaDadosPessoa = new ControladorBuscaDadosPessoa();
					ModelPessoa = await controladorBuscaDadosPessoa.ObterPessoaBuscaDadosCpf(ParametroCPF);
					if (ModelPessoa != null)
					{
						controladorPessoa.ManterPessoa(ModelPessoa);
					}
				}

				if (ModelPessoa == null)
				{
					controladorPessoa.ManterPessoa(new PessoaModel() { CPF = ParametroCPF });
					ModelPessoa = await controladorPessoa.ObterPessoaPeloCPF(ParametroCPF);
				}

				DefinirLoaderMessage();

				ModelDadosCadastrais = Utilitarios.ConverterPara<DadosCadastraisModel>(ModelPessoa);
				ModelDadosSocioDemograficos = Utilitarios.ConverterPara<DadosSocioDemograficosModel>(ModelPessoa);
				ModelDadosCondicoesSaude = Utilitarios.ConverterPara<DadosCondicoesSaudeModel>(ModelPessoa);
				ModelDadosFamilia = Utilitarios.ConverterPara<DadosFamiliaModel>(ModelPessoa);

				InserirNomeSocial = !string.IsNullOrWhiteSpace(ModelDadosCadastrais.NomeSocial);

				Estado_OnChage(null);
				PossuiDeficiencia_OnChange(null);
				Nacionalidade_OnChage(ModelDadosCadastrais.CodNacionalidade);
				RacaCor_OnChage(null);
				ResponsavelFamilia_OnChange(null);

				EstadoNascimento_OnChage(null);
				await ObterUnidade();

				ModelResultadoValidacaoPessoa = new ControladorValidacaoPessoa().ObterResultadoValidacaoPessoa(ParametroCPF, false);
				ExibirErrosValidacaoCadastro();
			}
			catch (Exception ex)
			{

			}
			IsBusy = false;
		}

		public void ManterPessoa()
		{
			ModelPessoa.CompletarDados(ModelDadosCadastrais);
			ModelPessoa.CompletarDados(ModelDadosSocioDemograficos);
			ModelPessoa.CompletarDados(ModelDadosCondicoesSaude);
			ModelPessoa.CompletarDados(ModelDadosFamilia);

			new ControladorPessoa().ManterPessoa(ModelPessoa);
		}
		public void OnWizardFinish()
		{
			try
			{
				ShowWizard = false;
				ManterPessoa();

				new ControladorValidacaoPessoa().AtualizarValidacaoPessoa(ParametroCPF);
			}
			catch
			{
				ShowWizard = true;
			}
		}

		private void ExibirErrosValidacaoCadastro()
		{
			var erros = new Dictionary<string, List<string>>();

			if (ModelResultadoValidacaoPessoa != null)
			{
				if (ModelResultadoValidacaoPessoa.EstaValido == false)
				{
					foreach (var item in ModelResultadoValidacaoPessoa.ListaValidacaoPessoa)
					{
						erros.Add($"item_{ModelResultadoValidacaoPessoa.ListaValidacaoPessoa.IndexOf(item)}", new() { item.Mensagem });
					}
				}
			}

			if (erros.Any())
			{
				CustomValidation?.DisplayErrors(erros);
				StateHasChanged();
			}
		}
		public void OnRegistrationStepChange(WizardStepChangeEventArgs args)
		{
			CustomValidation?.ClearErrors();
			var erros = new Dictionary<string, List<string>>();

			var isFormValid = RegisterForm.IsValid();

			if (isFormValid)
			{
				if (RegisterForm.Model.GetType().Equals(typeof(DadosCadastraisModel)))
				{
					if (ModelDadosCadastrais.CodNacionalidade == ControladorDadosEsus.COD_NACIONALIDADE_BRASILEIRA
						|| ModelDadosCadastrais.CodNacionalidade == ControladorDadosEsus.COD_NACIONALIDADE_ESTRANGEIRO)
					{
						if (!ModelDadosCadastrais.CodPaisNascimento.HasValue)
						{
							string propName = nameof(ModelDadosCadastrais.CodPaisNascimento);
							erros.Add(propName, new() { ModelDadosCadastrais.GetErrorMessageFrom(propName) });
						}

						if (ModelDadosCadastrais.CodNacionalidade == ControladorDadosEsus.COD_NACIONALIDADE_BRASILEIRA)
						{
							if (!ModelDadosCadastrais.CodEstadoNascimento.HasValue)
							{
								string propName = nameof(ModelDadosCadastrais.CodEstadoNascimento);
								erros.Add(propName, new() { ModelDadosCadastrais.GetErrorMessageFrom(propName) });
							}

							if (string.IsNullOrWhiteSpace(ModelDadosCadastrais.CodIbgeMunicipioNascimento))
							{
								string propName = nameof(ModelDadosCadastrais.CodIbgeMunicipioNascimento);
								erros.Add(propName, new() { ModelDadosCadastrais.GetErrorMessageFrom(propName) });
							}
						}

						if (ModelDadosCadastrais.CodNacionalidade == ControladorDadosEsus.COD_NACIONALIDADE_ESTRANGEIRO)
						{
							if (!ModelDadosCadastrais.DataEtradaBrasil.HasValue)
							{
								string propName = nameof(ModelDadosCadastrais.DataEtradaBrasil);
								erros.Add(propName, new() { ModelDadosCadastrais.GetErrorMessageFrom(propName) });
							}
						}
					}
					else
					{
						if (!ModelDadosCadastrais.DataNaturalizacao.HasValue)
						{
							string propName = nameof(ModelDadosCadastrais.DataNaturalizacao);
							erros.Add(propName, new() { ModelDadosCadastrais.GetErrorMessageFrom(propName) });
						}

						if (string.IsNullOrWhiteSpace(ModelDadosCadastrais.PortariaNaturalizacao))
						{
							string propName = nameof(ModelDadosCadastrais.PortariaNaturalizacao);
							erros.Add(propName, new() { ModelDadosCadastrais.GetErrorMessageFrom(propName) });
						}
					}
				}

				if (RegisterForm.Model.GetType().Equals(typeof(DadosCondicoesSaudeModel)))
				{
					if (ModelDadosCondicoesSaude.PossuiDeficiencia && ModelDadosCondicoesSaude.ListaQualDeficiencia?.Any() != true)
					{
						string propName = nameof(ModelDadosCondicoesSaude.QualDeficiencia);
						erros.Add(propName, new() { ModelDadosCondicoesSaude.GetErrorMessageFrom(propName) });
					}

					if (ModelDadosCondicoesSaude.PossuiOutraCondicaoSaude && string.IsNullOrWhiteSpace(ModelDadosCondicoesSaude.QualOutraCondicaoSaude))
					{
						string propName = nameof(ModelDadosCondicoesSaude.QualOutraCondicaoSaude);
						erros.Add(propName, new() { ModelDadosCondicoesSaude.GetErrorMessageFrom(propName) });
					}
				}

				if (RegisterForm.Model.GetType().Equals(typeof(DadosFamiliaModel)))
				{
					if (ModelDadosFamilia.DesejaInformarResponsavelFamilia && !Utilitarios.ValidarCPF(ModelDadosFamilia.CpfResponsavel))
					{
						string propName = nameof(ModelDadosFamilia.CpfResponsavel);
						erros.Add(propName, new() { ModelDadosFamilia.GetErrorMessageFrom(propName) });
					}
				}

				var pessoaValidacao = new PessoaModel();
				pessoaValidacao.CompletarDados(RegisterForm.Model);

				foreach (var item in new ControladorPessoa().ValidarDadosPessoa(pessoaValidacao))
				{
					erros.Append(item);
				}

				//if (ModelResultadoValidacaoPessoa != null)
				//{
				//	if (ModelResultadoValidacaoPessoa.EstaValido == false)
				//	{
				//		List<ValidacaoPessoaModel> validacoesExibidas = new List<ValidacaoPessoaModel>();
				//		foreach (var item in ModelResultadoValidacaoPessoa.ListaValidacaoPessoa)
				//		{
				//			if (RegisterForm.Model.GetType().GetProperty(item.AtributoInvalido) != null)
				//			{
				//				erros.Add(item.AtributoInvalido, new() { item.Mensagem });
				//				validacoesExibidas.Add(item);
				//			}
				//		}
				//		foreach (var item in validacoesExibidas)
				//		{
				//			ModelResultadoValidacaoPessoa.ListaValidacaoPessoa.Remove(item);
				//		}
				//	}
				//}
			}

			if (erros.Any())
			{
				isFormValid = false;
				CustomValidation?.DisplayErrors(erros);
			}

			if (!isFormValid)
			{
				args.IsCancelled = true;
			}
			else
			{
				ManterPessoa();
			}
		}
		public void Stepper_OnChange(int index)
		{
			if (index < WizardStep)
			{
				PreviousClick(WizardStep);
			}
			else
			{
				NextClick(WizardStep);
			}
		}

		public void NextClick(int currentStepIndex)
		{
			var args = new WizardStepChangeEventArgs()
			{
				IsCancelled = false,
				TargetIndex = currentStepIndex + 1
			};

			OnRegistrationStepChange(args);

			if (!args.IsCancelled)
			{
				WizardStep = currentStepIndex + 1;
			}

			ExibirErrosValidacaoCadastro();
		}
		public void PreviousClick(int newStepIndex)
		{
			WizardStep = newStepIndex - 1;

			ExibirErrosValidacaoCadastro();
		}


		public bool ValidSubmit { get; set; } = false;

		public void HandleInvalidSubmit()
		{
			ValidSubmit = false;
		}

		public void Estado_OnChage(object estado)
		{
			OpcoesMunicipio = new ControladorEndereco().ObterMunicipios(ModelDadosCadastrais.CodEstado);
		}

		public void RacaCor_OnChage(object racaCor)
		{
			if (ModelDadosCadastrais.CodRacaCor == ControladorDadosEsus.COD_RACACOR_INDIGENA)
			{
				ExibirDivEtnia = true;
				OpcoesEtnia = new ControladorDadosEsus().ObterOpcoesEtnia();
			}
			else
			{
				ExibirDivEtnia = false;
				ModelDadosCadastrais.CodEtnia = null;
			}

			StateHasChanged();
		}

		public bool HabilitarSelecaoPaisNascimento { get; set; }
		public void Nacionalidade_OnChage(object codNacionalidade)
		{
			ModelDadosCadastrais.CodNacionalidade = (long?)codNacionalidade;
			HabilitarSelecaoPaisNascimento = true;

			if (ModelDadosCadastrais.CodNacionalidade != null)
			{
				if (ModelDadosCadastrais.CodNacionalidade == ControladorDadosEsus.COD_NACIONALIDADE_BRASILEIRA)
				{
					ModelDadosCadastrais.CodPaisNascimento = ControladorDadosEsus.COD_BRASIL;
					HabilitarSelecaoPaisNascimento = false;
					EstadoNascimento_OnChage(null);
				}
				else
				{
					ModelDadosCadastrais.CodPaisNascimento = null;
				}
			}

			StateHasChanged();
		}

		public void EstadoNascimento_OnChage(object estado)
		{
			OpcoesMunicipioNascimento = new ControladorEndereco().ObterMunicipios(ModelDadosCadastrais.CodEstadoNascimento);
		}

		public void PossuiDeficiencia_OnChange(object deficiencia)
		{
			if (ModelDadosCondicoesSaude.PossuiDeficiencia == true)
			{
				ExibirDivQualDeficiencia = true;
				OpcoesDeficiencias = new ControladorDadosEsus().ObterOpcoesDeficiencias();

				if (ModelDadosCondicoesSaude.ListaQualDeficiencia?.Any() == true)
				{
					foreach (var item in ModelDadosCondicoesSaude.ListaQualDeficiencia)
					{
						OpcoesDeficiencias.Where(d => d.Codigo == item).ToList().ForEach(d => d.IsSelecionado = true);
					}
				}
			}
			else
			{
				ExibirDivQualDeficiencia = false;
				ModelDadosCondicoesSaude.QualDeficiencia = null;
			}
		}

		public void ResponsavelFamilia_OnChange(object responsavel)
		{
			if (ModelDadosFamilia.ResponsavelFamilia == true)
			{
				ModelDadosFamilia.DesejaInformarResponsavelFamilia = false;
				ModelDadosFamilia.CodRelacaoParentescoResponsavel = null;
				DesejaInformarResponsavelFamilia_OnChange(false);
			}
		}
		public void DesejaInformarResponsavelFamilia_OnChange(object deseja)
		{
			if (ModelDadosFamilia.DesejaInformarResponsavelFamilia != true)
			{
				ModelDadosFamilia.CpfResponsavel = null;
			}
		}

		public void QualDeficiencia_OnChange(object qualDeficiencia)
		{
			List<long> deficiencias = new List<long>();

			foreach (var item in OpcoesDeficiencias.Where(d => d.IsSelecionado == true).ToList())
			{
				deficiencias.Add(item.Codigo);
			}

			ModelDadosCondicoesSaude.ListaQualDeficiencia = deficiencias;
		}

		public string MascaraTelefoneContato { get; set; }

		public void TelefoneContato_OnBlur()
		{
			if (!string.IsNullOrWhiteSpace(ModelDadosCadastrais?.TelefoneContato))
			{
				ModelDadosCadastrais.TelefoneContato = ModelDadosCadastrais.TelefoneContato.Trim();
			}
		}

		public string ObterLabelCssClass(bool isRequired)
		{
			return isRequired ? "required-label" : "custom-label";
		}

		public bool UtilizarCepBuzy { get; set; }
		public async Task UtilizarCep_OnClink()
		{
			UtilizarCepBuzy = true;
			CepModel cepDTO = null;

			if (!string.IsNullOrWhiteSpace(ModelDadosCadastrais.CEP) && ModelDadosCadastrais.CEP.Trim().Length == 8)
			{
				ControladorEndereco controladorEndereco = new ControladorEndereco();
				cepDTO = await controladorEndereco.ObterEnderecoCep(ModelDadosCadastrais.CEP);

				if (cepDTO?.IsSucesso == true)
				{
					ModelDadosCadastrais.CodEstado = cepDTO.Estado?.CodEstado;
					Estado_OnChage(ModelDadosCadastrais.CodEstado);
					ModelDadosCadastrais.CodMunicipioIbge = cepDTO.CodMunicipioIbge;
					ModelDadosCadastrais.Bairro = cepDTO.Bairro;
					ModelDadosCadastrais.Logradouro = cepDTO.Logradouro;
					ModelDadosCadastrais.Complemento = cepDTO.Complemento;

					StateHasChanged();

					await ObterUnidade();
				}
			}

			if (cepDTO?.IsSucesso != true)
			{
				ExibirNotificacaoErro(cepDTO?.MensagemErro ?? "CEP inválido!");
			}

			UtilizarCepBuzy = false;
		}

		public async Task ObterUnidade()
		{
			ControladorUnidadeSaude controladorUnidadeSaude = new ControladorUnidadeSaude();
			UnidadeSaudeModel unidade = null;

			if (string.IsNullOrWhiteSpace(ModelDadosCadastrais.CnesReferencia))
			{
				if (!string.IsNullOrWhiteSpace(ModelDadosCadastrais.CEP) && ModelDadosCadastrais.CEP.Trim().Length == 8)
				{
					unidade = await controladorUnidadeSaude.DescobrirUnidadeSaude(ModelDadosCadastrais.CEP);
				}
			}
			else
			{
				unidade = controladorUnidadeSaude.ObterUnidadeSaude(ModelDadosCadastrais.CnesReferencia);
			}


			if (unidade != null)
			{
				ModelDadosCadastrais.DescricaoCnesReferencia = unidade.Descricao;
				ModelDadosCadastrais.CnesReferencia = unidade.CNES;
			}

			StateHasChanged();
		}

		private void ExibirNotificacaoErro(string mensagem)
		{
			NotificationComponent.Show(new NotificationModel
			{
				Text = mensagem,
				ThemeColor = "error",
				CloseAfter = 5000
			});
		}

		private void DefinirLoaderMessage(string mensagem = "")
		{
			LoaderMessage = "Carregando, por favor aguarde...";
			if (!string.IsNullOrWhiteSpace(mensagem))
			{
				LoaderMessage = mensagem;
			}

			StateHasChanged();
		}
	}
}
