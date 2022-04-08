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
	public class IndexBase : ComponentBase
	{
		[Inject] private NavigationManager? _navigationManager { get; set; }

	}
}
