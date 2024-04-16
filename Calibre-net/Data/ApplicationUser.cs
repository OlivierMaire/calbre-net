using Microsoft.AspNetCore.Identity;

namespace Calibre_net.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{

    public virtual List<UserPermission> Permissions {get;set;} = [];

    public string PreferredLocale {get;set;} = string.Empty;
}

