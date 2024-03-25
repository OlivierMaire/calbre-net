using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class GenericCategoryCount
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
 

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("sort")]
    public string? Sort { get; set; }

    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;

    // [JsonPropertyName("books")]
    // public List<BookDto> Books {get;set;} = [];

    [JsonPropertyName("searchUrl")]
    public string SearchUrl {get;set;} = string.Empty;

}

