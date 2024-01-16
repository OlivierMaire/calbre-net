using System.Security.Claims;
using calibre_net.Data;
using HeimGuard;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

public class UserPolicyHandler : IUserPolicyHandler
{
    // private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext dbContext;

    public UserPolicyHandler(//AuthenticationStateProvider authenticationStateProvider,
    IHttpContextAccessor _httpContextAccessor,
    ApplicationDbContext dbContext)
    {
        // this.authenticationStateProvider = authenticationStateProvider;
        this._httpContextAccessor = _httpContextAccessor;
        this.dbContext = dbContext;
    }
    
    public async Task<IEnumerable<string>> GetUserPermissions()
    {
        // var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = _httpContextAccessor?.HttpContext?.User;
        // var user = authState?.User;
        if (user == null) throw new ArgumentNullException(nameof(user));
        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
      
      	// this gets their permissions based on their roles. in this example, it's just using a static list
        var permissions = (await dbContext.UserPermissions
            .Where(p => p.UserId == userId)
            .Select(p => p.PermissionName)
            .FromCacheAsync("permissions", userId))
            .ToArray();

        // On Permission update Expire the tag with:
        // QueryCacheManager.ExpireTag("permissions", userId);

        return permissions;
    }
}