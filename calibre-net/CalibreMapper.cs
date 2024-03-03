using Riok.Mapperly.Abstractions;
using calibre_net.Shared.Contracts;
using Calibre_net.Data.Calibre;

// Mapper declaration
[Mapper]
[UseStaticMapper(typeof(BookDtoMapper))]
public static partial class BookMapper
{
    public static partial IQueryable<BookDto> ProjectToDto(this IQueryable<Book> query);
    public static partial IEnumerable<BookDto> ProjectToDto(this IEnumerable<Book> query);
    public static partial BookDto ToDto(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Authors))]
    public static partial BookDto ToDtoWithoutAuthors(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Series))]
    public static partial BookDto ToDtoWithoutSeries(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Rating))]
    public static partial BookDto ToDtoWithoutRatings(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Languages))]
    public static partial BookDto ToDtoWithoutLanguages(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Identifiers))]
    public static partial BookDto ToDtoWithoutIdentifiers(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Tags))]
    public static partial BookDto ToDtoWithoutTags(this Book book);
    [MapperIgnoreTarget(nameof(BookDto.Publisher))]
    public static partial BookDto ToDtoWithoutPublisher(this Book book);
}

public static class BookDtoMapper
{
    public static AuthorDto MapAuthor(Author author)
       => author.ToDtoWithoutBooks();
    public static SeriesDto MapSeriesList(ICollection<Series> series)
       => series.Count > 0 ? series.First().ToDtoWithoutBooks() : new SeriesDto();
    public static RatingDto MapRatingsList(ICollection<Rating> ratings)
       => ratings.Count > 0 ? ratings.First().ToDtoWithoutBooks() : new RatingDto();
    public static LanguageDto MapLanguage(Language language)
       => language.ToDtoWithoutBooks();
    public static IdentifierDto MapIdentifier(Identifier identifier)
       => identifier.ToDtoWithoutBooks();
    public static TagDto MapIdentifier(Tag tag)
       => tag.ToDtoWithoutBooks();
    public static PublisherDto MapPublishersList(ICollection<Publisher> publishers)
    => publishers.Count > 0 ? publishers.First().ToDtoWithoutBooks() : new PublisherDto();
    public static CustomColumnDto MapCustomColumn(CustomColumn customColumn)
        => customColumn.ToDtoWithoutBooks();

    public static IDictionary<int, CustomColumnDto> MapCustomColumnDictionary(IDictionary<int, CustomColumn> dict)
      =>  dict.ToDictionaryMap();
    
}

[Mapper]
[UseStaticMapper(typeof(AuthorBookDtoMapper))]
public static partial class AuthorMapper
{
    public static partial IQueryable<AuthorDto> ProjectToDto(this IQueryable<Author> query);
    public static partial IEnumerable<AuthorDto> ProjectToDto(this IEnumerable<Author> query);
    public static partial AuthorDto ToDto(this Author book);
    [MapperIgnoreTarget(nameof(AuthorDto.Books))]
    public static partial AuthorDto ToDtoWithoutBooks(this Author book);
}

public static class AuthorBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutAuthors();
}

[Mapper]
[UseStaticMapper(typeof(SeriesBookDtoMapper))]
public static partial class SeriesMapper
{
    public static partial IQueryable<SeriesDto> ProjectToDto(this IQueryable<Series> query);
    public static partial IEnumerable<SeriesDto> ProjectToDto(this IEnumerable<Series> query);
    public static partial SeriesDto ToDto(this Series book);
    [MapperIgnoreTarget(nameof(SeriesDto.Books))]
    public static partial SeriesDto ToDtoWithoutBooks(this Series book);
}

public static class SeriesBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutSeries();
}

[Mapper]
[UseStaticMapper(typeof(RatingBookDtoMapper))]
public static partial class RatingMapper
{
    public static partial IQueryable<RatingDto> ProjectToDto(this IQueryable<Rating> query);
    public static partial IEnumerable<RatingDto> ProjectToDto(this IEnumerable<Rating> query);
    public static partial RatingDto ToDto(this Rating book);
    [MapProperty(nameof(Rating.RatingValue), nameof(RatingDto.Rating))]
    [MapperIgnoreTarget(nameof(RatingDto.Books))]
    public static partial RatingDto ToDtoWithoutBooks(this Rating book);
}

public static class RatingBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutRatings();
}

[Mapper]
[UseStaticMapper(typeof(LanguageBookDtoMapper))]
public static partial class LanguageMapper
{
    public static partial IQueryable<LanguageDto> ProjectToDto(this IQueryable<Language> query);
    public static partial IEnumerable<LanguageDto> ProjectToDto(this IEnumerable<Language> query);
    public static partial LanguageDto ToDto(this Language book);
    [MapperIgnoreTarget(nameof(LanguageDto.Books))]
    public static partial LanguageDto ToDtoWithoutBooks(this Language book);
}

public static class LanguageBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutLanguages();
}

[Mapper]
[UseStaticMapper(typeof(IdentifierBookDtoMapper))]
public static partial class IdentifierMapper
{
    public static partial IQueryable<IdentifierDto> ProjectToDto(this IQueryable<Identifier> query);
    public static partial IEnumerable<IdentifierDto> ProjectToDto(this IEnumerable<Identifier> query);
    public static partial IdentifierDto ToDto(this Identifier book);
    [MapperIgnoreTarget(nameof(IdentifierDto.BookObject))]
    public static partial IdentifierDto ToDtoWithoutBooks(this Identifier book);
}

public static class IdentifierBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutIdentifiers();
}


[Mapper]
[UseStaticMapper(typeof(TagBookDtoMapper))]
public static partial class TagMapper
{
    public static partial IQueryable<TagDto> ProjectToDto(this IQueryable<Tag> query);
    public static partial IEnumerable<TagDto> ProjectToDto(this IEnumerable<Tag> query);
    public static partial TagDto ToDto(this Tag book);
    [MapperIgnoreTarget(nameof(TagDto.Books))]
    public static partial TagDto ToDtoWithoutBooks(this Tag book);
}

public static class TagBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutTags();
}

[Mapper]
[UseStaticMapper(typeof(PublisherBookDtoMapper))]
public static partial class PublisherMapper
{
    public static partial IQueryable<PublisherDto> ProjectToDto(this IQueryable<Publisher> query);
    public static partial IEnumerable<PublisherDto> ProjectToDto(this IEnumerable<Publisher> query);
    public static partial PublisherDto ToDto(this Publisher book);
    [MapperIgnoreTarget(nameof(PublisherDto.Books))]
    public static partial PublisherDto ToDtoWithoutBooks(this Publisher book);
}

public static class PublisherBookDtoMapper
{
    public static BookDto MapBook(Book book)
       => book.ToDtoWithoutPublisher();
}

[Mapper]
// [UseStaticMapper(typeof(PublisherBookDtoMapper))]
public static partial class CustomColumnMapper
{
    public static partial IQueryable<CustomColumnDto> ProjectToDto(this IQueryable<CustomColumn> query);
    public static partial IEnumerable<CustomColumnDto> ProjectToDto(this IEnumerable<CustomColumn> query);
    public static partial CustomColumnDto ToDto(this CustomColumn book);
    // [MapperIgnoreTarget(nameof(CustomColumnDto.Books))]
    public static partial CustomColumnDto ToDtoWithoutBooks(this CustomColumn book);
public static partial IDictionary<int, CustomColumnDto> ToDictionaryMap(this IDictionary<int, CustomColumn> dict);
    public static partial GenericCustomColumnDto ToDto(this GenericCustomColumn book);
}

[Mapper]
// [UseStaticMapper(typeof(PublisherBookDtoMapper))]
public static partial class CommentMapper
{
    public static partial IQueryable<CommentDto> ProjectToDto(this IQueryable<Comment> query);
    public static partial IEnumerable<CommentDto> ProjectToDto(this IEnumerable<Comment> query);
    public static partial CommentDto ToDto(this Comment entity);
}

[Mapper]
// [UseStaticMapper(typeof(PublisherBookDtoMapper))]
public static partial class DataMapper
{
    public static partial IQueryable<DataDto> ProjectToDto(this IQueryable<Datum> query);
    public static partial IEnumerable<DataDto> ProjectToDto(this IEnumerable<Datum> query);
    public static partial DataDto ToDto(this Datum entity);
}
