using System.ComponentModel.DataAnnotations.Schema;

namespace Calibre_net.Data;

[Table("UserPermissions")]
public class UserPermission
{
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser User { get; set; } = null!;

    public string PermissionName { get; set; } = string.Empty;
}