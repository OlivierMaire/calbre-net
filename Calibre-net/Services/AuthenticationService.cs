using Calibre_net.Client.Models;
using Calibre_net.Client.Services;
using Calibre_net.Components.Account;
using Calibre_net.Data;
using Calibre_net.Middleware;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace Calibre_net.Services;

[ScopedRegistration]
public class CalibreNetAuthenticationService : IAuthenticationService
{
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly NavigationManager navigationManager;
    private readonly PasskeyService passkeyService;

    public CalibreNetAuthenticationService(SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    NavigationManager navigationManager,
    PasskeyService passkeyService
)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.navigationManager = navigationManager;
        this.passkeyService = passkeyService;
    }

    public async Task<Guid> SignInAsync(SignInModel data)
    {
        if (data == null)
            throw new ServiceException("Invalid Login Credentials", 401);
        var usr = userManager.Users.FirstOrDefault(u => u.Email == data.Email) ?? throw new ServiceException("User Not Found", 401);

        if (await signInManager.CanSignInAsync(usr))
        {
            // prepare for cookie middle ware
            var key = BlazorCookieAuthenticationMiddleware<ApplicationUser>.AnnounceSignIn(data);
            return key;
        }
        else
        {
            throw new ServiceException("Your account is blocked", 401);
        }
    }

    public async Task<Guid> SignInAsync(SignInModel data, byte[] passkeyCredentialId)
    {
        var usr = passkeyService.GetUserFromCredential(passkeyCredentialId) ?? throw new ServiceException("User Not Found", 401);

        if (await signInManager.CanSignInAsync(usr))
        {
            // prepare for cookie middle ware
            data.CredentialId = passkeyCredentialId;
            var key = BlazorCookieAuthenticationMiddleware<ApplicationUser>.AnnounceSignIn(data);
            return key;
        }
        else
        {
            throw new ServiceException("Your account is blocked", 401);
        }
    }

    public Task<AuthenticationModel> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        throw new NotImplementedException();
    }

}