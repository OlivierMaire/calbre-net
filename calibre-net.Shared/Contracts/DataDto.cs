using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class DataDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; } = string.Empty;

    [JsonPropertyName("book")]
    public int Book { get; set; }
    [JsonPropertyName("uncompressed_size")]
    public int UncompressedSize { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;


    [JsonIgnore]
    private  string[] ReadableFormat =[ "txt", "pdf", "epub", "cbz", "cbt", "cbr", "djvu", "djv" ]; 
    [JsonIgnore]
    public bool IsReadable => 
        ReadableFormat.Contains(Format.ToLowerInvariant());

    [JsonIgnore]
    private  string[] ListenableFormat =["mp3", "mp4", "ogg", "opus", "wav", "flac", "m4a", "m4b"]; 
    [JsonIgnore]
    public bool IsListenable => 
        ListenableFormat.Contains(Format.ToLowerInvariant());
    [JsonIgnore]
    public string DownloadUrl => $"/api/v1/book/download/{Book}/{Format}";
    [JsonIgnore]
    public string ReadUrl => $"/book/read/{Book}/{Format}";
}