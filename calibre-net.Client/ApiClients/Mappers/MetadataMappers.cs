

using calibre_net.Shared.Contracts;
using Riok.Mapperly.Abstractions;

[Mapper]
public static partial class MetadataMapper
{
    public static partial ComicsBlazor.ComicMetadata ToComicsBlazorMetadata(this ComicMetadata obj);
    public static partial AudioPlayerBlazor.AudioMetadata ToAudioPlayerBlazorMetadata(this AudioMetadata obj);


}

[Mapper]
public static partial class CategoryMapper
{
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<TagDto> query);
    [MapProperty(nameof(TagDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(TagDto book);
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<SeriesDto> query);
    [MapProperty(nameof(SeriesDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(SeriesDto book);
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<AuthorDto> query);
    [MapProperty(nameof(AuthorDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(AuthorDto book);
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<PublisherDto> query);

    [MapProperty(nameof(PublisherDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(PublisherDto book);
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<LanguageDto> query);
    [MapProperty(nameof(LanguageDto.LangCode), nameof(GenericCategoryCount.Name))]
    [MapProperty(nameof(LanguageDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(LanguageDto book);
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<RatingDto> query);
    [MapProperty(nameof(RatingDto.Rating), nameof(GenericCategoryCount.Name))]
    [MapProperty(nameof(RatingDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(RatingDto book);
    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<FormatDto> query);
    [MapProperty(nameof(FormatDto.Format), nameof(GenericCategoryCount.Name))]
    [MapProperty(nameof(FormatDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(FormatDto book);

    public static partial IEnumerable<GenericCategoryCount> ProjectToCategoryCount(this IEnumerable<GenericCustomColumnDto> query);
    [MapProperty(nameof(GenericCustomColumnDto.Value), nameof(GenericCategoryCount.Name))]
    [MapProperty(nameof(GenericCustomColumnDto.SearchUrl), nameof(GenericCategoryCount._searchUrl))]
    public static partial GenericCategoryCount Map(GenericCustomColumnDto book);
}

