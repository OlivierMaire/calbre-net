using System.Security.Authentication;
using System.Security.Claims;
using calibre_net.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Z.EntityFramework.Plus;

namespace calibre_net.Middleware;

public class CalibreNetClaimsPrincipalFactory :
       UserClaimsPrincipalFactory<ApplicationUser>
{
    private readonly ApplicationDbContext dbContext;

    public CalibreNetClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor,
        ApplicationDbContext dbContext)
            : base(userManager, optionsAccessor)
    {
        this.dbContext = dbContext;
    }

    // This method is called only when login. It means that "the drawback   
    // of calling the database with each HTTP request" never happen.  
    public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);

        if (principal.Identity != null)
        {
            // this gets their permissions based on their roles. in this example, it's just using a static list
            var permissions = (await dbContext.UserPermissions
                .Where(p => p.UserId == user.Id)
                .Select(p => p.PermissionName)
                .FromCacheAsync("permissions", user.Id))
                .ToArray();


            foreach (var p in permissions)
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(
                    new[] { new Claim("Permissions", p) });
            }

            var culture = user.PreferredLocale;

            ((ClaimsIdentity)principal.Identity).AddClaims(
                new[] { new Claim("preferedlocale", culture) });
        }



        return principal;
    }
}