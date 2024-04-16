using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;
public partial class MetadataDto
{

    public MetadataType MetaType { get; set; }

    public string Type { get; set; } = string.Empty;

    public object? Data { get; set; }
}



public enum MetadataType
{
    EPub,
    Audio
}
 