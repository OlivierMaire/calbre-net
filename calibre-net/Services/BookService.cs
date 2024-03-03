using calibre_net.Client;
using calibre_net.Client.Services;
using calibre_net.Data.Calibre;
using calibre_net.Shared.Contracts;
using Calibre_net.Data.Calibre;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace calibre_net.Services;

[ScopedRegistration]
public class BookService(
    CalibreDbDapperContext dbContext,
    CalibreDbContext calibreDb, CustomColumnService customColumnService)

{
    private readonly CalibreDbDapperContext dbContext = dbContext;
    private readonly CalibreDbContext calibreDb = calibreDb;
    private readonly CustomColumnService customColumnService = customColumnService;

    public List<BookDto> GetBooks()
    {
        // var books = calibreDb.Books
        // .Include(b => b.Authors)
        // .Include(b => b.Series)
        // .Include(b => b.Rating)
        // .AsSplitQuery()
        // .FromCache("books");

        using (var ctx = dbContext.ConnectionCreate())
        {

            var books =
   ctx.Query<Book, Author, Series, Rating, Book>($"""
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
            """, (book, author, serie, rating) =>
   {
       book.Authors.Add(author);
       book.Series.Add(serie);
       book.Rating.Add(rating);
       return book;
   }, splitOn: "Id,Id,Id,Id");

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


        // var customColumns = customColumnService.GetCustomColumns();

        // var customTables = customColumnService.GetCustomColumnTables();

        // var test = customTables[1].Include(ct => ct.ColumnLink)
        // .Where(cc => cc.ColumnLink.Any(cl => cl.Book == id)).ToList();

        // Console.WriteLine($"{test.Count}");
        // foreach (var t in test)
        // {

        //     Console.WriteLine($"{t.Value}");
        // }
        // calibreDb.Set<GenericCustomColumn>("custom_column_1").whe

        // var book = calibreDb.Books
        // .Include(b => b.Authors)
        // .Include(b => b.Series)
        // .Include(b => b.Rating)
        // .Include(b => b.Languages)
        // .Include(b => b.Identifiers)
        // .Include(b => b.Tags)
        // .Include(b => b.Publisher)
        // .AsSplitQuery()
        // .Where(b => b.Id == id)
        // .FromCache("book", id.ToString())
        // .FirstOrDefault();

        // if (book == null)
        //     return null;
        // return book.ToDto();
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
            return bookPath ?? string.Empty;
        }
        // var bookPath = calibreDb.Books
        // .Where(b => b.Id == id)
        // .Select(b => b.Path)
        // .FromCache("book_cover", id.ToString())
        // .FirstOrDefault();

        // return bookPath ?? string.Empty;
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
            """, new { id , format });
            var path = bookPath;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += fileName + "." + format.ToLowerInvariant();


            return path ;
        }
        
    }
}