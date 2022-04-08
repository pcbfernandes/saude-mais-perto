using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SMP.Dominio;
using SMP.Dominio.Controlador;
using SMP.Dominio.Model;
using System.ComponentModel.DataAnnotations;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Upload;

namespace SMP.Pages
{
	public class TipoCadastroBase : ComponentBase
	{
		[Inject] private NavigationManager _navigationManager { get; set; }
		public AcessoModel ModelAcesso { get; set; } = new();
		public CustomValidation? CustomValidation { get; set; }
		public bool ValidSubmit { get; set; } = false;

		public async void HandleValidSubmit()
		{
			try
			{
				CustomValidation?.ClearErrors();
				var erros = new Dictionary<string, List<string>>();

				if (Utilitarios.ValidarCPF(ModelAcesso.CPF))
				{
					ValidSubmit = true;
					_navigationManager.NavigateTo($"cadastro/{ModelAcesso.CPF}");
				}
				else
				{
					erros.Add(nameof(ModelAcesso.CPF), new() { "Informe um CPF válido" });
					ValidSubmit = false;
				}

				if (erros.Any())
				{
					CustomValidation?.DisplayErrors(erros);
				}

			}
			catch (Exception ex)
			{
				ValidSubmit = false;
			}

			StateHasChanged();
		}
		public void HandleInvalidSubmit()
		{
			ValidSubmit = false;
		}

	}
}
