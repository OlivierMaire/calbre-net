

using calibre_net.Shared.Contracts;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class MetadataMapper{
    public static partial ComicsBlazor.ComicMetadata ToComicsBlazorMetadata(this ComicMetadata obj);

}