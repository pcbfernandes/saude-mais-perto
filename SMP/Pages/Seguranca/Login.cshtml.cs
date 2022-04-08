using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SMP.Dominio;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SMP.Pages.Seguranca
{
	public class LoginModel : PageModel
	{
		[BindProperty]
		public string Login { get; set; }

		[BindProperty]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string MensagemErro { get; set; }
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl)
		{
			try
			{
				MensagemErro = string.Empty;

				if (Login.ToUpper() == AppConfig.Administrador.Login.ToUpper() && Password == AppConfig.Administrador.Senha)
				{
					var claims = new List<Claim>();
					claims.Add(new Claim(ClaimTypes.Name, AppConfig.Administrador.Nome));
					claims.Add(new Claim(ClaimTypes.NameIdentifier, AppConfig.Administrador.Login));
					claims.Add(new Claim(ClaimTypes.Role, "Administradores"));

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					AuthenticationProperties authProperties = new AuthenticationProperties()
					{
						ExpiresUtc = DateTime.Now.AddHours(8),
						IsPersistent = true,
					};

					await HttpContext.SignInAsync(
								   CookieAuthenticationDefaults.AuthenticationScheme,
								   new ClaimsPrincipal(claimsIdentity), authProperties);
				}
				else
				{
					throw new Exception("Usuário ou senha inválidos!");
				}

				return LocalRedirect(Url.Content("~/"));
			}
			catch (Exception erro)
			{
				MensagemErro = erro.Message;
			}

			return Page();
		}
	}
}
