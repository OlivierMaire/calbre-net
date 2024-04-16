using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;
public partial class TagDto: Searchable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
 

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;

    // [JsonPropertyName("books")]
    // public List<BookDto> Books {get;set;} = [];

    [JsonIgnore]
    [JsonPropertyName("searchUrl")]
    public override string SearchUrl => $"tag/{Id}"; 

}

