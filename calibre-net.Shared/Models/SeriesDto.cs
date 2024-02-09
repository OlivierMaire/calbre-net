using System.Text.Json.Serialization;

namespace calibre_net.Shared.Models;
public partial class SeriesDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("sort")]
    public string? Sort { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;

    [JsonPropertyName("books")]
    public List<BookDto> Books {get;set;} = [];

    
    [JsonIgnore]
    public string SeriesLink => $"/series/{Id}";

}