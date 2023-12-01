using calibre_net.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Globalization;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services.AddLocalization();

// await builder.Build().RunAsync();

builder.Services.AddLocalization();

var host = builder.Build();

// CultureInfo culture;
// var js = host.Services.GetRequiredService<IJSRuntime>();
// var result = await js.InvokeAsync<string>("blazorCulture.get");

// if (result != null)
// {
//     culture = new CultureInfo(result);
// }
// else
// {
//     culture = new CultureInfo("en-US");
//     await js.InvokeVoidAsync("blazorCulture.set", "en-US");
// }

// CultureInfo.DefaultThreadCurrentCulture = culture;
// CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();