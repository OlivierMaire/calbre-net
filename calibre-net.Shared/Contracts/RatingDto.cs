using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class RatingDto: Searchable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("rating")]
    public int? Rating { get; set; } = null;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("books")]
    public List<BookDto> Books {get;set;} = [];

    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;

    [JsonIgnore]
    public override string SearchUrl => $"rating/{Id}";
}