using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using calibre_net.Components.Account;
using calibre_net.Data;
using MudBlazor.Services;
using Microsoft.Extensions.Localization;
using calibre_net.Middleware;
using MudExtensions.Services;
using calibre_net.Services;
using calibre_net.Components;
using calibre_net.Shared.Resources;
using calibre_net.Client.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddControllers();


builder.Services.AddMudServices();
builder.Services.AddMudExtensions();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// builder.Services.AddLocalization();

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

builder.Services.AddSingleton
     <IMessageService, MessageService>();
builder.Services.AddScoped<WindowIdService>();


builder.Services.AddScoped<CalibreNetAuthenticationService>();
builder.Services.AddScoped<PasskeyService>();

// builder.Services.AddScoped<Fido2NetLib.Fido2Configuration>();
// builder.Services.AddScoped<Fido2NetLib.Fido2>();

builder.Services.AddFido2(options =>
      {
          options.ServerDomain = "localhost";
          options.ServerName = "Calibre.Net";
          options.ServerIcon = "https://static-00.iconduck.com/assets.00/apps-calibre-icon-512x512-qox1oz2k.png";
          options.Origins = new HashSet<string> { "https://localhost:7046/" };

          //   options.ServerDomain = Configuration["fido2:serverDomain"];
          //   options.ServerName = "FIDO2 Test";
          //   options.Origins = Configuration.GetSection("fido2:origins").Get<HashSet<string>>();
          //   options.TimestampDriftTolerance = Configuration.GetValue<int>("fido2:timestampDriftTolerance");
          //   options.MDSCacheDirPath = Configuration["fido2:MDSCacheDirPath"];
          //   options.BackupEligibleCredentialPolicy = Configuration.GetValue<Fido2Configuration.CredentialBackupPolicy>("fido2:backupEligibleCredentialPolicy");
          //   options.BackedUpCredentialPolicy = Configuration.GetValue<Fido2Configuration.CredentialBackupPolicy>("fido2:backedUpCredentialPolicy");
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
app.UseAntiforgery();

app.UseMiddleware<BlazorCookieAuthenticationMiddleware<ApplicationUser>>();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(calibre_net.Client.Pages.Counter).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();


app.Run();
