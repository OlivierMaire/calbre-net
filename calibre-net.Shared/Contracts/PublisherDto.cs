using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class PublisherDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
 

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("sort")]
    public string Sort { get; set; } = string.Empty;
    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;


    [JsonPropertyName("books")]
    public List<BookDto> Books {get;set;} = [];

    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;
    
    [JsonIgnore]
    public string SearchUrl => $"/books/publisher/{Id}";

}

