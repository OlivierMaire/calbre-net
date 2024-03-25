using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class GenericCustomColumnDto
{
    [JsonPropertyName("columnId")]
    public int ColumnId { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;

    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;

    
    [JsonPropertyName("bookCount")]
    public int BookCount {get;set;} = 0;

    [JsonIgnore]
    public string SearchUrl => $"/books/cc_{ColumnId}/{Id}";
}