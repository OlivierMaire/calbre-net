using calibre_net.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;
using Microsoft.JSInterop;
using calibre_net.Shared.Resources;
using Microsoft.Extensions.Localization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// await builder.Build().RunAsync();

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});
SupportedCulturesOptions supportedCulturesOptions = new SupportedCulturesOptions();
// builder.Services.Configure<SupportedCulturesOptions>(options => {});
builder.Services.Configure<JsonStringLocalizerOptions>(options =>
{
    options.ResourcesPath = "Resources";
    options.ShowKeyNameIfEmpty = true;
    options.ShowDefaultIfEmpty = true;

});
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

var host = builder.Build();

CultureInfo culture;
var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");

if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo(supportedCulturesOptions.SupportedCultures[0]);
    await js.InvokeVoidAsync("blazorCulture.set", "en-US");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();