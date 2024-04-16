using System.Text.Json.Serialization;

namespace Calibre_net.Models;

public class AaGuidModel{
    [JsonPropertyName("name")]
    public string Name {get;set;} = string.Empty;
    [JsonPropertyName("icon_dark")]
    public string IconDark {get;set;} = string.Empty;
    [JsonPropertyName("icon_light")]
    public string IconLight {get;set;} = string.Empty;
}