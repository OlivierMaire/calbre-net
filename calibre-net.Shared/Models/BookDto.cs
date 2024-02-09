using System.Text.Json.Serialization;

namespace calibre_net.Shared.Models;
public partial class BookDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("sort")]
    public string? Sort { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; set; }

    [JsonPropertyName("pubdate")]
    public DateTimeOffset? Pubdate { get; set; }

    [JsonPropertyName("seriesIndex")]
    public double SeriesIndex { get; set; }

    [JsonPropertyName("authorsort")]
    public string? AuthorSort { get; set; }

    // public string[] Authors => 
    //     this.AuthorSort?.Split("&", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    // }

    public string? Isbn { get; set; }

    public string? Lccn { get; set; }

    [JsonConverter(typeof(SensibleDataConverter<string>))]
    [JsonPropertyName("path")]
    public string Path { get; set; } = null!;

    public int Flags { get; set; }

    public string? Uuid { get; set; }

    [JsonPropertyName("hascover")]
    public bool? HasCover { get; set; }

    [JsonPropertyName("lastmodified")]
    public DateTimeOffset LastModified { get; set; }

    [JsonPropertyName("authors")]
    public List<AuthorDto> Authors {get;set;} = [];
    [JsonPropertyName("series")]
    public SeriesDto Series {get;set;} = null!;


    [JsonPropertyName("rating")]
    public RatingDto Ratings {get;set;} = null!;

    [JsonIgnore]
    public string BookLink => $"/book/{Id}";
}
