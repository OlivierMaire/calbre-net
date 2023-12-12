using calibre_net.Client.Models;
using calibre_net.Client.Services;
using calibre_net.Components.Account;
using calibre_net.Data;
using calibre_net.Middleware;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace calibre_net.Services;

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

    public async Task<Guid?> SignInAsync(SignInModel data)
    {
        if (data == null)
            throw new ServiceException("Invalid Login Credentials", 401);
        string error;
        error = null;
        var usr = userManager.Users.FirstOrDefault(u => u.Email == data.Email);
        if (usr == null)
        {
            throw new ServiceException("User Not Found", 401);
        }

        if (await signInManager.CanSignInAsync(usr))
        {
            // var result = await signInManager.CheckPasswordSignInAsync(usr, data.Password, true);
            // if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                // prepare for cookie middle ware
                var key = BlazorCookieAuthenticationMiddleware<ApplicationUser>.AnnounceSignIn(data);
                // NavMgr.NavigateTo($"/login?key={key}", true);

                // add a token in the session
                // var token = await this.CreateTokenAsync(usr);
                // await pLocalStorage.SetAsync("SessionToken", token);
                // await localStorage.SetItemAsync("SessionToken", token);
                return key;
            }
            // else if (result.RequiresTwoFactor)
            // {
            //     var newUri = navigationManager.GetUriWithQueryParameters("Account/LoginWith2fa", (Dictionary<string, object?>)new() { ["returnUrl"] = data.ReturnUrl, ["rememberMe"] = data.RememberMe } );
            //     navigationManager.NavigateTo(newUri);
            // }
            // else
            // {
            //     throw new ServiceException("Login failed. Check your password.", 401);
            // }
        }
        else
        {
            throw new ServiceException("Your account is blocked", 401);
        }

        return null;
    }

     public async Task<Guid?> SignInAsync(SignInModel data, byte[] passkeyCredentialId)
    {
        string error;
        error = null;
        var usr = passkeyService.GetUserFromCredential(passkeyCredentialId);
        if (usr == null)
        {
            throw new ServiceException("User Not Found", 401);
        }

        if (await signInManager.CanSignInAsync(usr))
        {
            // var result = await signInManager.CheckPasswordSignInAsync(usr, data.Password, true);
            // if (result == Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                // prepare for cookie middle ware
                data.CredentialId = passkeyCredentialId;
                var key = BlazorCookieAuthenticationMiddleware<ApplicationUser>.AnnounceSignIn(data);
                // NavMgr.NavigateTo($"/login?key={key}", true);

                // add a token in the session
                // var token = await this.CreateTokenAsync(usr);
                // await pLocalStorage.SetAsync("SessionToken", token);
                // await localStorage.SetItemAsync("SessionToken", token);
                return key;
            }
            // else if (result.RequiresTwoFactor)
            // {
            //     var newUri = navigationManager.GetUriWithQueryParameters("Account/LoginWith2fa", (Dictionary<string, object?>)new() { ["returnUrl"] = data.ReturnUrl, ["rememberMe"] = data.RememberMe } );
            //     navigationManager.NavigateTo(newUri);
            // }
            // else
            // {
            //     throw new ServiceException("Login failed. Check your password.", 401);
            // }
        }
        else
        {
            throw new ServiceException("Your account is blocked", 401);
        }

        return null;
    }

    public async Task<AuthenticationModel?> RefreshTokenAsync(string accessToken, string refreshToken)
    {

        // var principal = GetPrincipalFromExpiredToken(accessToken);
        // if (principal == null)
        // {
        //     logger.LogError("Invalid access token or refresh token");
        //     return null;
        //     // return BadRequest("Invalid access token or refresh token");
        // }

        // var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        // if (emailClaim is null)
        //     return null;

        // string email = emailClaim.Value;

        // var user = await this.userManager.FindByEmailAsync(email);

        // // if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        // // {
        // //     logger.LogError("Invalid access token or refresh token 2");
        // //     return null;
        // //     //return BadRequest("Invalid access token or refresh token");
        // // }

        // // _user = user;
        // return await CreateTokenAsync(user, false);

        return null;
    }

    // public async Task<ApplicationUser> GetCurrentUser()
    // {


    // }

}