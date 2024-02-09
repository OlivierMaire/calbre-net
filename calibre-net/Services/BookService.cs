using calibre_net.Client.Services;
using calibre_net.Shared.Models;
using Calibre_net.Data.Calibre;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace calibre_net.Services;

[ScopedRegistration]
public class BookService(CalibreDbContext calibreDb)

{
    private readonly CalibreDbContext calibreDb = calibreDb;

    public List<BookDto> GetBooks()
    {
        var books = calibreDb.Books
        .Include(b => b.Authors)
        .Include(b => b.Series)
        .Include(b => b.Ratings)
        .FromCache("books");
        
        return books.ProjectToDto().ToList();
    }

    public BookDto? GetBook(int id)
    {
        var book = calibreDb.Books
        .Include(b => b.Authors)
        .Include(b => b.Series)
        .Include(b => b.Ratings)
        .Where(b => b.Id == id)
        .FromCache("book", id.ToString())
        .FirstOrDefault();

        if (book == null)
            return null;
        return book.ToDto();
    }

}