using System.Globalization;
using System.Security.Claims;
using calibre_net.Data;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;

namespace calibre_net.Api.Endpoints;

public sealed class SetCultureEndpoint : Endpoint<SetCultureRequest>
{
    public override void Configure()
    {
        Get("culture/set");
        AllowAnonymous();
        RoutePrefixOverride("");
    }

    public override async Task HandleAsync(SetCultureRequest req, CancellationToken ct)
    {
        if (req.Culture != null)
        {
            this.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(req.Culture, req.Culture)));

            // HttpContext.Response.Cookies.Append(
            //     CookieRequestCultureProvider.DefaultCookieName,
            //     CookieRequestCultureProvider.MakeCookieValue(
            //         new RequestCulture(culture, culture)));

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(req.Culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(req.Culture);
        }

        await SendRedirectAsync(req.RedirectUri);
    }
}

public record SetCultureRequest(string Culture, string RedirectUri);

public sealed class GetCultureEndpoint(
UserManager<ApplicationUser> userManager) : EndpointWithoutRequest<GetCultureResponse>
{
    public override void Configure()
    {
        Get("culture/get");
        RoutePrefixOverride("");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var user = await userManager.GetUserAsync(User);
        await SendOkAsync(new GetCultureResponse(string.IsNullOrEmpty(user?.PreferredLocale) ? "en-US" : user.PreferredLocale), ct);
    }
}

