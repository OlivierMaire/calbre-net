using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;
public partial class CommentDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    [JsonPropertyName("book")]
    public int book { get; set; }

}