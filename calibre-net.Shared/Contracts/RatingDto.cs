using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;
public partial class RatingDto : Searchable, IComparable
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("rating")]
    public int? Rating { get; set; } = null;

    [JsonPropertyName("link")]
    public string Link { get; set; } = string.Empty;

    [JsonPropertyName("books")]
    public List<BookDto> Books { get; set; } = [];

    [JsonPropertyName("bookCount")]
    public int BookCount { get; set; } = 0;

    [JsonIgnore]
    public override string SearchUrl => $"rating/{Id}";

    public int CompareTo(object? obj)
    {
        if (obj != null && obj is RatingDto rating)
        {
            if (this.Rating == null && rating.Rating == null)
                return 0;

            return this.Rating?.CompareTo(rating.Rating) ?? -1;
        }
        return -1;
    }
}