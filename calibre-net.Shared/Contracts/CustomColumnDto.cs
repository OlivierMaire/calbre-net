using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class CustomColumnDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("datatype")]
    public string Datatype { get; set; } = null!;

    [JsonPropertyName("mark_for_delete")]
    public bool MarkForDelete { get; set; }

    [JsonPropertyName("editable")]
    public bool Editable { get; set; }

    [JsonPropertyName("display")]
    public string Display { get; set; } = null!;

    [JsonPropertyName("is_multiple")]
    public bool IsMultiple { get; set; }

    [JsonPropertyName("normalized")]
    public bool Normalized { get; set; }

    
    [JsonPropertyName("data")]
    public List<GenericCustomColumnDto> Data { get; set; }
}