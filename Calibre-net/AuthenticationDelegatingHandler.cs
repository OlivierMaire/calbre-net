
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Calibre_net;

public class ServerAuthenticationDelegatingHandler(IHttpContextAccessor accessor) : DelegatingHandler
{
    private readonly IHttpContextAccessor accessor = accessor;

    /// <summary>
    /// Main method to override for the handler.
    /// </summary>
    /// <param name="request">The original request.</param>
    /// <param name="cancellationToken">The token to handle cancellations.</param>
    /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // get cookies from the current HttpContext request.
        var cookie = accessor.HttpContext?.Request.Headers.Cookie;
        if (!string.IsNullOrEmpty(cookie.ToString()))
            // add the cookies to the HttpClient request.
            request.Headers.Add("Cookie", cookie.ToString());

        // Send Request
        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}
