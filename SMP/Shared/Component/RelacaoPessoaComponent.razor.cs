using Microsoft.AspNetCore.Components;
using SMP.Dominio;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;

namespace SMP.Shared.Component
{
	public class RelacaoPessoaComponentBase : ComponentBase
	{
		[Parameter] public string ParametroCpf { get; set; }
		[Parameter] public string ParametroCpfResponsavel { get; set; }
		[Parameter] public bool ParametroIsResponsavel { get; set; }
		[Parameter] public long? ParametroCodRelacaoParentescoResponsavel { get; set; }
		[Parameter] public bool ParametroDesejaInformarResponsavelFamilia { get; set; }
		public string CpfResponsavel { get; set; }
		public List<RelacionamentoPessoaModel> ListaRelacionamentoPessoa { get; set; } = new List<RelacionamentoPessoaModel>();

		public RelacionamentoPessoaModel ModelRelacionamentoPessoa { get; set; }
		private bool _isFirstRender { get; set; }
		protected override void OnAfterRender(bool firstRender)
		{
			_isFirstRender = firstRender;
			base.OnAfterRender(firstRender);
		}
		protected override void OnParametersSet()
		{
			CpfResponsavel = string.Empty;

			if (!string.IsNullOrWhiteSpace(ParametroCpfResponsavel) && Utilitarios.ValidarCPF(ParametroCpfResponsavel))
			{
				CpfResponsavel = ParametroCpfResponsavel;
			}
			else if (ParametroIsResponsavel)
			{
				CpfResponsavel = ParametroCpf;
			}

			if (!_isFirstRender)
			{
				if (string.IsNullOrWhiteSpace(ParametroCpfResponsavel) || Utilitarios.ValidarCPF(ParametroCpfResponsavel) || ParametroIsResponsavel)
				{
					new ControladorRelacionamento().ManterPessoaDadosFamilia(ParametroCpf, ParametroCpfResponsavel, ParametroCodRelacaoParentescoResponsavel, ParametroDesejaInformarResponsavelFamilia);
				}
			}

			AtualizarListaRelacionamento();
		}

		public void AtualizarListaRelacionamento()
		{
			ListaRelacionamentoPessoa = new ControladorRelacionamento().ObterListaRelacionamento(CpfResponsavel);
			StateHasChanged();
		}
	}
}
