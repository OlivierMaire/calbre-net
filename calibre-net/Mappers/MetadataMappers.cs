

using calibre_net.Shared.Contracts;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class MetadataMapper{
    public static partial AudioMetadata ToDto(this AudioPlayerBlazor.AudioMetadata obj);
    public static partial ComicMetadata ToDto(this ComicMeta.Metadata.GenericMetadata obj);

}