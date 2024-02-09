using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Calibre_net.Data.Calibre;

public partial class CalibreDbContext : DbContext
{

    [DbFunction("sortconcat")]
    public static string SortedConcatenate(string value)
        => throw new NotSupportedException();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity<BooksAuthorsLink>(
                l => l.HasOne<Author>().WithMany().HasForeignKey(bal => bal.Author).HasPrincipalKey(a => a.Id),
                r => r.HasOne<Book>().WithMany().HasForeignKey(bal => bal.Book).HasPrincipalKey(b => b.Id),
               j => j.HasKey(bal => new { bal.Book, bal.Author })
            );

        modelBuilder.Entity<Book>()
        .HasMany(b => b.Series)
        .WithMany(a => a.Books)
        .UsingEntity<BooksSeriesLink>(
            l => l.HasOne<Series>().WithMany().HasForeignKey(bsl => bsl.Series).HasPrincipalKey(a => a.Id),
            r => r.HasOne<Book>().WithMany().HasForeignKey(bsl => bsl.Book).HasPrincipalKey(b => b.Id),
           j => j.HasKey(bsl => new { bsl.Book, bsl.Series })
        );

        modelBuilder.Entity<Book>()
                   .HasMany(b => b.Ratings)
                   .WithMany(a => a.Books)
                   .UsingEntity<BooksRatingsLink>(
                       l => l.HasOne<Rating>().WithMany().HasForeignKey(brl => brl.Rating).HasPrincipalKey(a => a.Id),
                       r => r.HasOne<Book>().WithMany().HasForeignKey(brl => brl.Book).HasPrincipalKey(b => b.Id),
                      j => j.HasKey(brl => new { brl.Book, brl.Rating })
                   );
        // .UsingEntity<BooksAuthorsLink>();
        //     .UsingEntity<BooksAuthorsLink>(
        // // "PostTag",
        // l => l.HasOne(typeof(Tag)).WithMany().HasForeignKey("TagsId").HasPrincipalKey(nameof(Tag.Id)),
        // r => r.HasOne(typeof(Post)).WithMany().HasForeignKey("PostsId").HasPrincipalKey(nameof(Post.Id)),
        // j => j.HasKey("PostsId", "TagsId"));

    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     var connection = Configure("Name=ConnectionStrings:DevCalibreDb");

    //     // Passing in an already open connection will keep the connection open between
    //     // requests.
    //     optionsBuilder.UseSqlite(connection);
    // }

    public static SqliteConnection Configure(string connectionString)
    {
        var connection = new SqliteConnection(connectionString);

        connection.Open();

        connection.CreateFunction("sortconcat", (Func<string, string>)SortedConcatenate);
        return connection;
    }

}


public partial class Book
{
    virtual public ICollection<Author> Authors { get; set; } = null!;

    virtual public ICollection<Series>? Series { get; set; } = null;
    virtual public ICollection<Rating>? Ratings { get; set; } = null;
}

public partial class Author
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}

public partial class BooksAuthorsLink
{

    virtual public Author AuthorObject { get; set; } = null!;
    virtual public Book BookObject { get; set; } = null!;
}

public partial class Series
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}


public partial class Rating
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}


public partial class BooksSeriesLink
{
    virtual public Book BookObject { get; set; } = null!;
    virtual public Series SeriesObject { get; set; } = null!;

}

public partial class BooksRatingsLink
{
    virtual public Book BookObject { get; set; } = null!;
    virtual public Rating RatingObject { get; set; } = null!;

}