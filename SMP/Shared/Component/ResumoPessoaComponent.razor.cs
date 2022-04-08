using Microsoft.AspNetCore.Components;
using SMP.Dominio;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;

namespace SMP.Shared.Component
{
	public class ResumoPessoaComponentBase : ComponentBase
	{
		[Parameter] public string? ParametroCPF { get; set; }
		[Parameter] public PessoaModel? ModelPessoa { get; set; }
		[Parameter] public List<string>? ExibirApenas { get; set; }
		[Parameter] public int ColBotstrap { get; set; } = 5;
		[Parameter] public bool ExibirSituacaoCadastral { get; set; } = false;
		public Dictionary<string, string> Resumo { get; set; } = new Dictionary<string, string>();
		public DadosCadastraisModel ModelDadosCadastrais { get; set; } = new DadosCadastraisModel();
		public DadosSocioDemograficosModel ModelDadosSocioDemograficos { get; set; } = new DadosSocioDemograficosModel();
		public DadosCondicoesSaudeModel ModelDadosCondicoesSaude { get; set; } = new DadosCondicoesSaudeModel();
		public bool IsBusy { get; set; }

		protected override async Task OnInitializedAsync()
		{
			IsBusy = true;
			if (ModelPessoa == null)
			{
				var controladorPessoa = new ControladorPessoa();
				ModelPessoa = await controladorPessoa.ObterPessoaPeloCPF(ParametroCPF);
			}

			ModelDadosCadastrais = Utilitarios.ConverterPara<DadosCadastraisModel>(ModelPessoa);
			ModelDadosSocioDemograficos = Utilitarios.ConverterPara<DadosSocioDemograficosModel>(ModelPessoa);
			ModelDadosCondicoesSaude = Utilitarios.ConverterPara<DadosCondicoesSaudeModel>(ModelPessoa);

			Resumo.ObterResumo(ModelDadosCadastrais, ExibirApenas);
			Resumo.ObterResumo(ModelDadosSocioDemograficos, ExibirApenas);
			Resumo.ObterResumo(ModelDadosCondicoesSaude, ExibirApenas);
			IsBusy = false;
		}
	}
}
