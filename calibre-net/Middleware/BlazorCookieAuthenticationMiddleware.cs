using System.Collections.Concurrent;
using calibre_net.Client.Models;
using calibre_net.Data;
using calibre_net.Services;
using Microsoft.AspNetCore.Identity;

namespace calibre_net.Middleware;

public class BlazorCookieAuthenticationMiddleware<TUser> where TUser : class
{

    #region Static SignIn Cache

    static IDictionary<Guid, SignInModel> SignIns { get; set; }
    = new ConcurrentDictionary<Guid, SignInModel>();

    public static Guid AnnounceSignIn(SignInModel signInInfo)
    {
        var key = Guid.NewGuid();
        SignIns[key] = signInInfo;
        return key;
    }
    public static SignInModel? GetSignInProgress(string key) => GetSignInProgress(Guid.Parse(key));

    public static SignInModel? GetSignInProgress(Guid key) => SignIns.ContainsKey(key) ? SignIns[key] : null;

    #endregion

    private readonly RequestDelegate _next;

    public BlazorCookieAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context,
    SignInManager<ApplicationUser> signInMgr,
    UserManager<ApplicationUser> userMgr,
    PasskeyService passkeyService
    )
    {
        // intercept request
        if (context.Request.Path == "/Account/Login" && context.Request.Query.ContainsKey("key") && !string.IsNullOrEmpty(context.Request.Query["key"]))
        {
            var key = Guid.Parse(context.Request.Query["key"].ToString());
            var signInInfo = SignIns[key];

            if (signInInfo.CredentialId != null)
            {
                // passkey 
                var passkeyUser = passkeyService.GetUserFromCredential(signInInfo.CredentialId);
                if (passkeyUser != null)
                {
                    await signInMgr.SignInAsync(passkeyUser, true, "passkey");
                    if (string.IsNullOrEmpty(signInInfo.ReturnUrl))
                        context.Response.Redirect("/");
                    else
                        context.Response.Redirect("/" + signInInfo.ReturnUrl.TrimStart('/'));
                    return;
                }
                else
                {
                    await _next.Invoke(context);
                }
            }

            var user = await userMgr.FindByEmailAsync(signInInfo.Email);
            if (user == null)
            {
                context.Response.Redirect("/");
                return;
            }

            var result = await signInMgr.PasswordSignInAsync(user, signInInfo.Password, true, lockoutOnFailure: false);

            //Uncache password for security:
            signInInfo.Password = string.Empty;

            if (result.Succeeded)
            {
                SignIns.Remove(key);

                if (string.IsNullOrEmpty(signInInfo.ReturnUrl))
                    context.Response.Redirect("/");
                else
                    context.Response.Redirect("/" + signInInfo.ReturnUrl.TrimStart('/'));
                return;
            }
            else if (result.RequiresTwoFactor)
            {
                // context.Response.Redirect("/Account/LoginWith2fa/" + key);
                context.Response.Redirect("/Account/LoginWith2fa");
                return;
            }
            else if (result.IsLockedOut)
            {
                // info.Error = "You are locked out. Please contact support.";
            }
            else
            {
                // info.Error = "Login failed. Check your username and password.";
                await _next.Invoke(context);
            }
        }
        // else if (context.Request.Path.StartsWithSegments("/Account/loginwith2fa")  && context.Request.Query.ContainsKey("key") && context.Request.Path.HasValue)
        // {
        //     var key = Guid.Parse(context.Request.Path.Value.Split('/').Last());
        //     var info = SignIns[key];

        //     if (string.IsNullOrEmpty(info.TwoFactorCode))
        //     {
        //         //user is opening 2FA first time...
        //         //...Get user model and cache it for the 2FA-View:
        //         var user = await signInMgr.GetTwoFactorAuthenticationUserAsync();
        //         info.User = user;
        //     }
        //     else
        //     {
        //         //user has submitted 2FA, check:
        //         var result = await signInMgr.TwoFactorAuthenticatorSignInAsync(info.TwoFactorCode, info.RememberMe, info.RememberMachine);

        //         if (result.Succeeded)
        //         {
        //             SignIns.Remove(key);
        //             if (string.IsNullOrEmpty(info.ReturnUrl))
        //                 context.Response.Redirect("/");
        //             else
        //                 context.Response.Redirect("/" + info.ReturnUrl.TrimStart('/'));
        //             return;
        //         }
        //         else if (result.IsLockedOut)
        //         {
        //             // info.Error = "You are locked out. Please contact support.";
        //         }
        //         else
        //         {
        //             // info.Error = "Invalid authenticator code";
        //         }
        //     }
        // }
        else if (context.Request.Path.StartsWithSegments("/Account/SignOut"))
        {
            await signInMgr.SignOutAsync();
            context.Response.Redirect("/Account/Login");
            return;
        }

        //Continue http middleware chain:
        await _next.Invoke(context);
    }

}