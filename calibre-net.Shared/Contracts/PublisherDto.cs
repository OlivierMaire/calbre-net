using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class PublisherDto : Searchable, IComparable
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
    public List<BookDto> Books { get; set; } = [];

    [JsonPropertyName("bookCount")]
    public int BookCount { get; set; } = 0;

    [JsonIgnore]
    public override string SearchUrl => $"publisher/{Id}";


    public int CompareTo(object? obj)
    {
        if (obj != null && obj is SeriesDto series)
        {
            if (string.IsNullOrEmpty(this.Sort) && string.IsNullOrEmpty(series?.Sort))
                return 0;

            return this.Sort.CompareTo(series?.Sort);
        }
        return -1;
    }
}

