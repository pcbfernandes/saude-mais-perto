using Microsoft.AspNetCore.Components;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;

namespace SMP.Pages
{
	public class ValidarPessoaBase : ComponentBase
	{
		[Parameter] public string? ParametroCPF { get; set; }
		public event EventHandler? OnClose;
		public ResultadoValidacaoPessoaModel? ModelResultadoValidacaoPessoa { get; set; }
		public List<ValidacaoPessoaModel>? ListaValidacaoPessoa { get; set; }
		public ValidacaoPessoaModel? ModelValidacaoPessoa { get; set; }

		public Dictionary<string, string>? OpcoesAtributos { get; set; }
		public bool IsVisible { get; set; }
		public int ActiveTabIndex { get; set; } = 0;

		protected override void OnParametersSet()
		{
			if (!string.IsNullOrWhiteSpace(ParametroCPF))
			{
				ModelValidacaoPessoa = new ValidacaoPessoaModel() { CPF = ParametroCPF };
				ControladorValidacaoPessoa controladorValidacaoPessoa = new ControladorValidacaoPessoa();
				OpcoesAtributos = controladorValidacaoPessoa.ObterOpcoesAtributos();
				AtualizarDados();
				IsVisible = true;
			}
		}

		public void AtualizarDados()
		{
			ControladorValidacaoPessoa controladorValidacaoPessoa = new ControladorValidacaoPessoa();
			ModelResultadoValidacaoPessoa = controladorValidacaoPessoa.ObterResultadoValidacaoPessoa(ParametroCPF, true);

			ListaValidacaoPessoa = ModelResultadoValidacaoPessoa.ListaValidacaoPessoa?.Where(v => !v.DataVisualizacao.HasValue)?.ToList() ?? new List<ValidacaoPessoaModel>();

			if (ModelResultadoValidacaoPessoa.EstaValido == true)
			{
				ModelValidacaoPessoa = ListaValidacaoPessoa.First();
			}
			ActiveTabIndex = 0;
		}

		public bool ValidSubmit { get; set; } = false;

		public async void HandleValidSubmit()
		{
			ListaValidacaoPessoa.Add(ModelValidacaoPessoa);
			ModelValidacaoPessoa = new ValidacaoPessoaModel() { CPF = ParametroCPF };

			StateHasChanged();
		}
		public void HandleInvalidSubmit()
		{
			ValidSubmit = false;
		}

		public void Remover_OnClick(ValidacaoPessoaModel validacao)
		{
			ListaValidacaoPessoa.Remove(validacao);
		}

		public void Salvar()
		{
			ControladorValidacaoPessoa controladorValidacaoPessoa = new ControladorValidacaoPessoa();

			if (!ListaValidacaoPessoa.Any())
			{
				if (ModelValidacaoPessoa.PessoaValidada)
				{					
					ListaValidacaoPessoa.Add(ModelValidacaoPessoa);
				}
			}
			controladorValidacaoPessoa.SalvarValidacao(ParametroCPF, ListaValidacaoPessoa);
			AtualizarDados();
		}

		public void VisibleChangedHandler(bool currVisible)
		{
			IsVisible = currVisible;
			if (!IsVisible)
			{
				ParametroCPF = string.Empty;
				if(OnClose != null)
				{
					OnClose(this, EventArgs.Empty);
				}
			}
		}
	}
}
