
namespace calibre_net.Shared.Models;

public class UserModel
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string> Permissions { get; set; } = [];
    public string PreferredLocale { get; set; }
}

public class UserModelExtended: UserModel{

    public string Password {get;set;}
    public Dictionary<string, string>? PermissionsDictionary { get; set; }
}