using Microsoft.AspNetCore.Components;
using SMP.Dominio;
using SMP.Dominio.Controlador;

namespace SMP.Shared.Component
{
	public class PreviaXmlComponentBase : ComponentBase
	{
		[Parameter] public string? ParametroCPF { get; set; }
		public event EventHandler? OnClose;
		public string PreviaXml { get; set; }
		public bool IsBusy { get; set; }
		public bool IsVisible { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			IsBusy = true;

			if (!string.IsNullOrWhiteSpace(ParametroCPF))
			{
				IsVisible = true;
				if (System.Diagnostics.Debugger.IsAttached)
				{
					await Task.Delay(3000);
				}

				ControladorDadosEsus controladorDadosEsus = new ControladorDadosEsus();
				var xml = await new FichaCadastroIndividual().ObterXml(ParametroCPF);
				PreviaXml = System.Xml.Linq.XDocument.Parse(xml.ToString(), System.Xml.Linq.LoadOptions.PreserveWhitespace).ToString();
				
			}

			IsBusy = false;
		}
		public void Fechar()
		{
			ParametroCPF = string.Empty;
			PreviaXml = string.Empty;
		}

		public void VisibleChangedHandler(bool currVisible)
		{
			IsVisible = currVisible;
			if (!IsVisible)
			{
				ParametroCPF = string.Empty;
				if (OnClose != null)
				{
					OnClose(this, EventArgs.Empty);
				}
			}
		}
	}
}
