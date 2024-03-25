using calibre_net.Client.Services;
using calibre_net.Data.Calibre;
using calibre_net.Shared.Contracts;
using Calibre_net.Data.Calibre;
using Dapper;

namespace calibre_net.Services;

[ScopedRegistration]
public class BookService(CalibreDbDapperContext dbContext)

{
    private readonly CalibreDbDapperContext dbContext = dbContext;

    public List<BookDto> GetBooks(SearchRequest req)
    {
        // var books = calibreDb.Books
        // .Include(b => b.Authors)
        // .Include(b => b.Series)
        // .Include(b => b.Rating)
        // .AsSplitQuery()
        // .FromCache("books");

        using var ctx = dbContext.ConnectionCreate();
        var dynamicParams = new DynamicParameters();
        var sql = $"""
            SELECT b.id, b.title, b.sort, b.timestamp, b.pubdate, b.series_index, b.author_sort, b.isbn, b.lccn, b.path, b.flags, b.uuid, b.has_cover, b.last_modified, 
            a.id, a.name, a.sort, a.link, 
            s.id, s.name, s.sort, s.link, 
            r.id, r.rating as ratingValue, r.link
            FROM books as b
            LEFT JOIN books_authors_link bal on bal.book = b.id
            LEFT JOIN authors a on a.Id = bal.author
            LEFT JOIN books_series_link bsl on bsl.book = b.id
            LEFT JOIN series s on s.Id = bsl.series
            LEFT JOIN books_ratings_link brl on brl.book = b.id
            LEFT JOIN ratings r on r.Id = brl.rating 
            """;

        string sqlJoin = string.Empty;
        string sqlWhere = string.Empty;

        if (req.Author.HasValue)
        {
            sqlWhere += " AND (a.Id = @authorId) ";
            dynamicParams.Add("authorId", req.Author);
        }
        if (req.Series.HasValue)
        {
            sqlWhere += " AND (s.Id = @seriesId) ";
            dynamicParams.Add("seriesId", req.Series);
        }
        if (req.Rating.HasValue)
        {
            sqlWhere += " AND (r.Id = @ratingId) ";
            dynamicParams.Add("ratingId", req.Rating);
        }
        if (req.RatingValue.HasValue)
        {
            sqlWhere += req.RatingValueOperator switch
            {
                SqlOperator.Equals => " AND (r.rating = @ratingValue) ",
                SqlOperator.GreaterThan => " AND (r.rating > @ratingValue) ",
                SqlOperator.LesserThan => " AND (r.rating < @ratingValue) ",
                SqlOperator.GreaterOrEquals => " AND (r.rating >= @ratingValue) ",
                SqlOperator.LesserOrEquals => " AND (r.rating <= @ratingValue) ",
                _ => " AND (r.rating = @ratingValue) ",
            };
            dynamicParams.Add("ratingValue", req.RatingValue);
        }
        if (req.Tag.HasValue)
        {
            sqlJoin += """ 
            JOIN books_tags_link btl on btl.book = b.id
            JOIN tags t on t.Id = btl.tag
            """;
            sqlWhere += " AND (t.Id = @tagId) ";
            dynamicParams.Add("tagId", req.Tag);
        }
        if (req.Publisher.HasValue)
        {
            sqlJoin += """ 
            JOIN books_publishers_link bpl on bpl.book = b.id
            JOIN publishers p on p.Id = bpl.publisher
            """;
            sqlWhere += " AND (p.Id = @publisherId) ";
            dynamicParams.Add("publisherId", req.Publisher);
        }
        if (req.Language.HasValue)
        {
            sqlJoin += """ 
            JOIN books_languages_link bll on bll.book = b.id
            JOIN languages l on l.Id = bll.lang_code
            """;
            sqlWhere += " AND (l.Id = @languageId) ";
            dynamicParams.Add("languageId", req.Language);
        }
        if (!string.IsNullOrEmpty(req.Format))
        {
            sqlJoin += """ 
            JOIN data d on d.book = b.id
            """;
            sqlWhere += " AND (d.Format = @formatValue) ";
            dynamicParams.Add("formatValue", req.Format);
        }

        foreach (var cc in req.CustomColumn)
        {
            if (cc.Value.HasValue)
            {
                sqlJoin += $""" 
                JOIN books_custom_column_{cc.Key}_link bll on bll.book = b.id
                JOIN custom_column_{cc.Key} cc_{cc.Key} on cc_{cc.Key}.Id = bll.value
                """;
                sqlWhere += $" AND (cc_{cc.Key}.Id = @cc_{cc.Key}Id) ";
                dynamicParams.Add($"cc_{cc.Key}Id", cc.Value);
            }
        }

        sql += sqlJoin;
        if (!string.IsNullOrEmpty(sqlWhere))
        {
            sql += " WHERE 1 = 1 ";
            sql += sqlWhere;
        }

        Console.WriteLine(sql);

        var books =
            ctx.Query<Book, Author, Series, Rating, Book>(sql,
                        (book, author, serie, rating) =>
            {
                book.Authors.Add(author);
                book.Series.Add(serie);
                book.Rating.Add(rating);
                return book;
            },
            param: dynamicParams,
            splitOn: "Id,Id,Id,Id");

        books = books.GroupBy(b => b.Id)
            .Select(g =>
            {
                var groupedBook = g.First();
                groupedBook.Authors = g.Select(b => b.Authors.SingleOrDefault() ?? new Author()).ToList();
                groupedBook.Series = g.Select(b => b.Series.SingleOrDefault() ?? new Series()).ToList();
                groupedBook.Rating = g.Select(b => b.Rating.SingleOrDefault() ?? new Rating()).ToList();
                return groupedBook;
            }).ToList();

        return books.ProjectToDto().ToList();

    }

    public BookDto? GetBook(int id)
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            using (var multi = ctx.QueryMultiple("""
                SELECT * from books where id = @id;
                SELECT a.* from books_authors_link bal join authors a on a.id = bal.author where bal.book = @id;
                SELECT s.* from books_series_link bsl join series s on s.Id = bsl.series where bsl.book = @id;
                SELECT r.id, r.rating as ratingValue, r.link from books_ratings_link brl join ratings r on r.Id = brl.rating where brl.book = @id;
                SELECT l.* from books_languages_link bll join languages l on l.Id = bll.lang_code where bll.book = @id;
                SELECT i.* from identifiers i where i.book = @id;
                SELECT t.* from books_tags_link btl join tags t on t.Id = btl.tag where btl.book = @id;
                SELECT p.* from books_publishers_link bpl join publishers p on p.Id = bpl.publisher where bpl.book = @id;
                SELECT c.* from comments c where c.book = @id;
                SELECT d.* from data d where d.book = @id;
                SELECT cc.* from custom_columns as cc;
                """, new { id }))
            {
                var book = multi.ReadFirst<Book>();

                if (book == null)
                    return null;

                book.Authors = multi.Read<Author>().ToList();
                book.Series = multi.Read<Series>().ToList();
                book.Rating = multi.Read<Rating>().ToList();
                book.Languages = multi.Read<Language>().ToList();
                book.Identifiers = multi.Read<Identifier>().ToList();
                book.Tags = multi.Read<Tag>().ToList();
                book.Publisher = multi.Read<Publisher>().ToList();
                book.Comments = multi.Read<Comment>().FirstOrDefault();
                book.Data = multi.Read<Datum>().ToList();

                var cc = multi.Read<CustomColumn>().ToDictionary(cc => cc.Id, cc => cc);
                book.CustomColumns = cc;
                var sql = string.Empty;
                foreach (var col in cc)
                {
                    sql += $" SELECT cc.* from books_custom_column_{col.Key}_link as bccl join custom_column_{col.Key} cc on cc.Id = bccl.value where bccl.book = @id; ";
                }
                if (!string.IsNullOrEmpty(sql))
                {
                    using (var multi_cc = ctx.QueryMultiple(sql, new { id }))
                    {

                        foreach (var col in cc)
                        {
                            book.CustomColumns[col.Key].Data = multi_cc.Read<GenericCustomColumn>().ToList();
                        }
                    }
                }

                return book.ToDto();
            }
        }

    }

    public string GetBookCover(int id)
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var bookPath =
            ctx.QueryFirstOrDefault<string>($"""
            SELECT b.path FROM books as b
            WHERE b.Id == @id
            """, new { id });
            var path = bookPath ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += "cover.jpg";
            return path;
        }
    }

    public string GetBookFile(int id, string format)
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var (bookPath, fileName) =
            ctx.QueryFirstOrDefault<(string, string)>($"""
            SELECT b.path, d.name FROM books as b
            JOIN data as d on d.book = b.id
            WHERE b.Id == @id
            and d.format = @format
            """, new { id, format });
            var path = bookPath;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += fileName + "." + format.ToLowerInvariant();
            return path;
        }

    }

    public List<TagDto> GetAllTags()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var tags =
            ctx.Query<TagDto>($"""
            select t.Id, MAX(t.name) as name, MAX(t.link) as link, count() as bookCount from tags t
            join books_tags_link btl on btl.tag = t.Id
            group by t.Id
            """);
            return tags.ToList();
        }
    }


    public List<SeriesDto> GetAllSeries()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var series =
            ctx.Query<SeriesDto>($"""
            select s.Id, MAX(s.name) as name, MAX(s.sort) as sort, MAX(s.link) as link, count() as bookCount from series s
            join books_series_link bsl on bsl.series = s.Id
            group by s.Id
            """);
            return series.ToList();
        }
    }

    public List<AuthorDto> GetAllAuthors()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var authors =
            ctx.Query<AuthorDto>($"""
            select s.Id, MAX(s.name) as name, MAX(s.sort) as sort, MAX(s.link) as link, count() as bookCount from authors s
            join books_authors_link bsl on bsl.author = s.Id
            group by s.Id
            """);
            return authors.ToList();
        }
    }

    public List<PublisherDto> GetAllPublishers()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var publishers =
            ctx.Query<PublisherDto>($"""
            select s.Id, MAX(s.name) as name, MAX(s.sort) as sort, MAX(s.link) as link, count() as bookCount from publishers s
            join books_publishers_link bsl on bsl.publisher = s.Id
            group by s.Id
            """);
            return publishers.ToList();
        }
    }
    public List<LanguageDto> GetAllLanguages()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var languages =
            ctx.Query<LanguageDto>($"""
            select s.Id, MAX(s.lang_code) as langCode, MAX(s.link) as link, count() as bookCount from languages s
            join books_languages_link bsl on bsl.lang_code = s.Id
            group by s.Id
            """);
            return languages.ToList();
        }
    }
    public List<RatingDto> GetAllRatings()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var ratings =
            ctx.Query<RatingDto>($"""
            select s.Id, MAX(s.rating) as rating, MAX(s.link) as link, count() as bookCount from ratings s
            join books_ratings_link bsl on bsl.rating = s.Id
            group by s.Id
            """);
            return ratings.ToList();
        }
    }

    public List<FormatDto> GetAllFormats()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var formats =
            ctx.Query<FormatDto>($"""
            select d.format, count() as bookCount from data d
            group by d.format
            """);
            return formats.ToList();
        }
    }

    public List<CustomColumn> GetCustomColumns()
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var cc =
            ctx.Query<CustomColumn>($"""
            select * from custom_columns
            """);
            return cc.ToList();
        }
    }

    public List<GenericCustomColumnDto> GetAllCustomColumns(int columnId)
    {
        using (var ctx = dbContext.ConnectionCreate())
        {
            var cc =
            ctx.Query<GenericCustomColumnDto>($"""
            select {columnId} as columnId, s.Id, MAX(s.value) as value, MAX(s.link) as link, count() as bookCount from custom_column_{columnId} s
            join books_custom_column_{columnId}_link bsl on bsl.value = s.Id
            group by s.Id
            """);
            return cc.ToList();
        }
    }
}