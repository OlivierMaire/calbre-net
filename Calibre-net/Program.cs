using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Calibre_net.Components.Account;
using Calibre_net.Data;
using MudBlazor.Services;
using Microsoft.Extensions.Localization;
using Calibre_net.Middleware;
using MudExtensions.Services;
using Calibre_net.Services;
using Calibre_net.Components;
using HeimGuard;
using Calibre_net.Shared.Resources;
using Calibre_net.Client.Services;
using Namotion.Reflection;
using Calibre_net.Migrations;
using Calibre_net.Shared.Contracts;
using Calibre_net.Data.Calibre;
using EPubBlazor;
using AudioPlayerBlazor;
using ComicsBlazor;
using FastEndpoints;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using Calibre_net;
using System.Security.Claims;
using MudBlazor;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
builder.Configuration.AddJsonFile("customsettings.json", optional: true, reloadOnChange: true);

var listeningPort = builder.Configuration["calibre:basic:server:port"];
if (!string.IsNullOrEmpty(listeningPort))
    builder.WebHost.UseUrls($"https://localhost:{listeningPort}");


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// builder.Services.AddControllers();


builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CalibreNetClaimsPrincipalFactory>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    })
    .AddIdentityCookies();


//  builder.Services.ConfigureApplicationCookie(options => {
//             options.AccessDeniedPath = "/Account/Login3333";
//  });

// builder.Services.ConfigureApplicationCookie(options =>
// options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents()
// {
//     OnRedirectToReturnUrl = (response) =>
//         {
//             if (response.Request.Path.StartsWithSegments("/api") && response.RedirectUri.Contains("Account/Login"))
//             {
//                 response.Response.StatusCode = 401;
//             }
//             return Task.CompletedTask;
//         },
//     OnRedirectToLogin = (response) =>
//    {
//        if (response.Request.Path.StartsWithSegments("/api") && response.Response.StatusCode == 200)
//        {
//            response.Response.StatusCode = 401;
//        }
//        return Task.CompletedTask;
//    },
//     OnRedirectToAccessDenied = (response) =>
//   {
//       if (response.Request.Path.StartsWithSegments("/api") && response.Response.StatusCode == 200)
//       {
//           response.Response.StatusCode = 403;
//       }
//       return Task.CompletedTask;
//   }
// });

builder.Services.AddHeimGuard<UserPolicyHandler>()
    .AutomaticallyCheckPermissions()
    .MapAuthorizationPolicies();

// builder.Services.AddAuthorizationCore(options =>
// {
//     // options.AddPolicy("Admin",
//     //     policy => policy.RequireClaim("Permissions", "Admin"));
//     options.AddPolicy("IdIsMyself", policy => policy.RequireClaim(ClaimTypes.NameIdentifier,  policy.))
// });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddLocalization();
SupportedCulturesOptions supportedCulturesOptions = new SupportedCulturesOptions();
// builder.Services.Configure<SupportedCulturesOptions>(options =>
// options.SupportedCultures = supportedCultures);
builder.Services.Configure<JsonStringLocalizerOptions>(options =>
{
    options.ResourcesPath = "Resources";
    options.ShowKeyNameIfEmpty = true;
    options.ShowDefaultIfEmpty = true;

});
builder.Services.AddSingleton
     <IStringLocalizerFactory, JsonStringLocalizerFactory>();

builder.Services.AddScoped<WindowIdService>();


builder.Services.AddScoped<CalibreNetAuthenticationService>();
builder.Services.AddScoped<PasskeyService>();

builder.Services.AddEPubBlazor(ServiceLifetime.Scoped);
// builder.Services.AddAudioPlayerBlazor(ServiceLifetime.Singleton);
builder.Services.AddAudioPlayerBlazor(ServiceLifetime.Scoped);
builder.Services.AddComicsBlazor(ServiceLifetime.Scoped);

builder.Services.RegisterServices(builder.Configuration);

builder.Services.Configure<CalibreConfiguration>(builder.Configuration.GetSection(CalibreConfiguration.SectionName));

var fidoRpId = builder.Configuration["calibre:basic:security:passkey_rpid"];
if (string.IsNullOrEmpty(fidoRpId))
fidoRpId = "localhost";
builder.Services.AddFido2(options =>
      {
          options.ServerDomain = fidoRpId; // <- Set front side domain
          options.ServerName = "Calibre.Net";
          options.ServerIcon = "https://static-00.iconduck.com/assets.00/apps-calibre-icon-512x512-qox1oz2k.png";
          options.Origins = new HashSet<string> { $"https://{fidoRpId}/"  };

          //   options.ServerDomain = Configuration["fido2:serverDomain"];
          //   options.ServerName = "FIDO2 Test";
          //   options.Origins = Configuration.GetSection("fido2:origins").Get<HashSet<string>>();
          //   options.TimestampDriftTolerance = Configuration.GetValue<int>("fido2:timestampDriftTolerance");
          //   options.MDSCacheDirPath = Configuration["fido2:MDSCacheDirPath"];
          //   options.BackupEligibleCredentialPolicy = Configuration.GetValue<Fido2Configuration.CredentialBackupPolicy>("fido2:backupEligibleCredentialPolicy");
          //   options.BackedUpCredentialPolicy = Configuration.GetValue<Fido2Configuration.CredentialBackupPolicy>("fido2:backedUpCredentialPolicy");
      });

builder.Services.AddFastEndpoints()
.AddResponseCaching()
.AddAntiforgery();


builder.Services
.SwaggerDocument(o =>
{
    o.ShortSchemaNames = true;
    // o.ExcludeNonFastEndpoints = true;
    o.MaxEndpointVersion = 1;
    o.DocumentSettings = s =>
    {
        s.Version = "v1";
        s.DocumentName = "v1";
        s.Title = "Calibre.Net Api";
        s.Description = "Calibre.Net Api Services.";
    };

});

var baseAddress = builder.Configuration["Calibre:ApiHost"];
if (string.IsNullOrEmpty(baseAddress))
{
    baseAddress = $"https://localhost:{listeningPort}";
}

builder.Services.AddHttpClient();


builder.Services.AddHttpClient("AuthenticationApi", client => client.BaseAddress = new Uri(baseAddress));


builder.Services.AddScoped<ServerAuthenticationDelegatingHandler>();
builder.Services.AddHttpClient("calibre-net.Api", client => client.BaseAddress = new Uri(baseAddress))
    .AddHttpMessageHandler<ServerAuthenticationDelegatingHandler>();

builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("custompolicy", MyCustomPolicy.Instance);
    // options.AddPolicy ("custompolicy", p => p.AddPolicy<MyCustomPolicy>().Exp);
});

builder.WebHost.ConfigureKestrel(o =>
{
    // o.Limits.
    o.Limits.MaxResponseBufferSize = null;
    // o.Limits.MaxRequestBodySize = 1073741824; //set to max allowed file size of your system
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCulturesOptions.SupportedCultures[0])
    .AddSupportedCultures(supportedCulturesOptions.SupportedCultures)
    .AddSupportedUICultures(supportedCulturesOptions.SupportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<BlazorCookieAuthenticationMiddleware<ApplicationUser>>();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();


app.UseAuthentication() //add this
   .UseAuthorization(); //add this

app.UseAntiforgery(); // should be after UseAuthentication

app.UseOutputCache();

app.UseResponseCaching()
// .UseAntiforgeryFE()
.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;
    c.Endpoints.RoutePrefix = "api";
});
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

// app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Calibre_net.Client.Pages.Book.Books).Assembly);





app.Run();
