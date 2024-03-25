using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class LanguageDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("langCode")]
    public string LangCode { get; set; } = string.Empty;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("books")]
    public List<BookDto> Books {get;set;} = [];

    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;

    [JsonIgnore]
    public string SearchUrl => $"/books/language/{Id}";

}