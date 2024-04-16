using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;
public partial class FormatDto : Searchable
{

    [JsonPropertyName("format")]
    public string Format { get; set; } = string.Empty;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;
    
    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;

    [JsonIgnore]
    public override string SearchUrl => $"format/{Format}";
}