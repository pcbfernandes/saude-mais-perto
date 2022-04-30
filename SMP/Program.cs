using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using SMP.Core;
using SMP.Dominio.Seguranca;
using SMP.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddTelerikBlazor();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var appConfigModel = new SMP.Dominio.AppConfigModel();
builder.Configuration.GetSection("AppConfig").Bind(appConfigModel);
SMP.Dominio.AppConfig.GetInstance(appConfigModel);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				 .AddCookie(options =>
				 {
					 options.Cookie.Name = "SAUDE.MAIS.PERTO";
					 options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
					 options.ExpireTimeSpan = TimeSpan.FromHours(8);
					 options.LoginPath = new PathString("/Seguranca/Login");
					 options.SlidingExpiration = true;
				 });

builder.Services.AddScoped<TokenProvider>();
builder.Services.AddAuthorization(config =>
{
	config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
	DefaultRequestCulture = new RequestCulture(defaultCulture),
	SupportedCultures = new List<CultureInfo> { defaultCulture },
	SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpContext();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

app.Run();
