using Calibre_net.Data;
using Calibre_net.Shared.Contracts;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class CustomPageMapper{

    public static partial IQueryable<CustomPageDto> ProjectToDto(this IQueryable<CustomPage> query);
    public static partial IEnumerable<CustomPageDto> ProjectToDto(this IEnumerable<CustomPage> query);
    [MapperIgnoreSource(nameof(CustomPage.Owner))]
    [UserMapping(Default = true)]
    public static partial CustomPageDto ToDto(this CustomPage entity); 

}