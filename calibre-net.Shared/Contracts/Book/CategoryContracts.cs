namespace calibre_net.Shared.Contracts;

public record GetTagsResponse(List<TagDto> Tags);
public record GetSeriesResponse(List<SeriesDto> Series);
public record GetAuthorsResponse(List<AuthorDto> Authors);
public record GetPublishersResponse(List<PublisherDto> Publishers);
public record GetLanguagesResponse(List<LanguageDto> Languages);
public record GetRatingsResponse(List<RatingDto> Rating);
public record GetFormatsResponse(List<FormatDto> Formats);
public record GetCustomColumnsResponse(List<GenericCustomColumnDto> CustomColumns);
public record GetCustomColumnsRequest(int ColumnId);

