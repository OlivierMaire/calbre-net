using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class TagDto
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
    public string SearchUrl => $"/books/tag/{Id}"; 

}

