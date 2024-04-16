
using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;

public class UserModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("permissions")]
    public List<string> Permissions { get; set; } = [];
    [JsonPropertyName("preferedlocale")]
    public string PreferredLocale { get; set; } = string.Empty;
}

public class UserModelExtended: UserModel{

    [JsonPropertyName("password")]
    public string Password {get;set;} = string.Empty;
    [JsonPropertyName("permission-dictionary")]
    public Dictionary<string, string>? PermissionsDictionary { get; set; }
}