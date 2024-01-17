
using System.Text.Json.Serialization;

namespace calibre_net.Shared.Models;

public class UserModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = [];
    [JsonPropertyName("preferedlocale")]
    public string PreferredLocale { get; set; }
}

public class UserModelExtended: UserModel{

    [JsonPropertyName("password")]
    public string Password {get;set;} = string.Empty;
    [JsonPropertyName("permission-dictionary")]
    public Dictionary<string, string>? PermissionsDictionary { get; set; }
}