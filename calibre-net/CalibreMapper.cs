using Riok.Mapperly.Abstractions;
using calibre_net.Shared.Models;
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
    [MapperIgnoreTarget(nameof(BookDto.Ratings))]
    public static partial BookDto ToDtoWithoutRatings(this Book book);
}

public static class BookDtoMapper{
 public static AuthorDto MapAuthor(Author author)
    => author.ToDtoWithoutBooks();
//  public static SeriesDto MapAuthor(Series series)
//     => series.ToDtoWithoutBooks();
    
 public static SeriesDto MapSeriesList(ICollection<Series> series)
    => series.Count > 0 ? series.First().ToDtoWithoutBooks() : new SeriesDto() ;

    
 public static RatingDto MapSeriesList(ICollection<Rating> ratings)
    => ratings.Count > 0 ? ratings.First().ToDtoWithoutBooks() : new RatingDto() ;
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

public static class AuthorBookDtoMapper{
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

public static class SeriesBookDtoMapper{
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
    [MapProperty(nameof(Rating.Rating1), nameof(RatingDto.Rating))]
    [MapperIgnoreTarget(nameof(RatingDto.Books))]
    public static partial RatingDto ToDtoWithoutBooks(this Rating book);
}

public static class RatingBookDtoMapper{
 public static BookDto MapBook(Book book)
    => book.ToDtoWithoutRatings();
}