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

    [JsonIgnore]
    public string SearchUrl => $"/language/{Id}";

}