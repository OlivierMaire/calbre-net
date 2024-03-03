using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class GenericCustomColumnDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;

    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;

    
}