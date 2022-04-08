using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SMP.Util;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;
using Telerik.Blazor.Components;
using SMP.Shared.Component;

namespace SMP.Pages
{
	public class RelatorioBase : ComponentBase
	{
		[Inject] private NavigationManager _navigationManager { get; set; }
		[Inject] private IJSRuntime _jSRuntime { get; set; }
		public List<PessoaModel> ListaPessoa { get; set; }
		public List<ResumoPessoaComponent> ListaResumo { get; set; }
		public List<string> ExibirApenas { get; set; }
		public string CpfSelecionado { get; set; }
		public string CpfSelecionadoValidacao { get; set; }
		public ValidarPessoa TelaValidarPessoa { get; set; }
		public PreviaXmlComponent TelaPreviaXml { get; set; }
		public int? FiltroSelecionado { get; set; }
		public Dictionary<string, int?> FiltroOpcoes { get; set; } = new Dictionary<string, int?>()
		{
			{ "Todos", null },
			{ "Pendentes", 1 },
			{ "Válidos", 2 },
			{ "Inválidos", 3 }
		};

		protected override Task OnInitializedAsync()
		{
			Atualizar();
			ExibirApenas = new List<string>() { "Nome", "CPF", "Sexo", "DataNascimento", "CnesReferencia" };		

			return base.OnInitializedAsync();
		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				TelaValidarPessoa.OnClose += TelaValidarPessoa_OnClose;
				TelaPreviaXml.OnClose += TelaPreviaXml_OnClose; ;
			}
			return base.OnAfterRenderAsync(firstRender);
		}

		public void Atualizar()
		{
			ListaPessoa = new ControladorPessoa().ObterListaPessoas();
		}

		public void VisualizarArquivo(ListViewCommandEventArgs args)
		{
			PessoaModel pessoa = (PessoaModel)args.Item;
			ControladorArquivo controladorArquivo = new ControladorArquivo();

			var dados = controladorArquivo.ObterArquivoDadosModel(pessoa.ModelArquivo.IdArquivoDados.Value);

			FileExporter.Save(_jSRuntime, dados?.Dados, "application/pdf", pessoa.ModelArquivo.Nome);
		}
		public void Editar(ListViewCommandEventArgs args)
		{
			_navigationManager.NavigateTo($"cadastro/{((PessoaModel)args.Item).CPF}");

		}
		public void VisualizarXml(ListViewCommandEventArgs args)
		{
			CpfSelecionado = ((PessoaModel)args.Item).CPF;
		}
		public void Validar(ListViewCommandEventArgs args)
		{
			CpfSelecionadoValidacao = ((PessoaModel)args.Item).CPF;
		}
		private void TelaValidarPessoa_OnClose(object? sender, EventArgs e)
		{
			CpfSelecionadoValidacao = string.Empty;
			Atualizar();
			StateHasChanged();
		}
		private void TelaPreviaXml_OnClose(object? sender, EventArgs e)
		{
			CpfSelecionado = string.Empty;
			StateHasChanged();
		}

	}
}
