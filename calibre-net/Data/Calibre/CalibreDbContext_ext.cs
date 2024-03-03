using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using MudBlazor.Extensions;

namespace Calibre_net.Data.Calibre;

public partial class CalibreDbContext : DbContext
{
    private string ConnectionString = string.Empty;

    // public virtual DbSet<Dictionary<int,GenericCustomColumn>> GenericCustomColumns { get; set; }
    // public virtual DbSet<GenericCustomColumn> GenericCustomColumns { get; set; }
    // public virtual DbSet<GenericBooksCustomColumnLink> GenericBooksCustomColumnLink { get; set; }

    public virtual Dictionary<int, DbSet<GenericCustomColumn>> CustomColumnsTables { get; set; } = new Dictionary<int, DbSet<GenericCustomColumn>>();

    [DbFunction("sortconcat")]
    public static string SortedConcatenate(string value)
        => throw new NotSupportedException();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // var connectionString = this.Database.GetConnectionString();


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
                   .HasMany(b => b.Rating)
                   .WithMany(a => a.Books)
                   .UsingEntity<BooksRatingsLink>(
                       l => l.HasOne<Rating>().WithMany().HasForeignKey(brl => brl.Rating).HasPrincipalKey(a => a.Id),
                       r => r.HasOne<Book>().WithMany().HasForeignKey(brl => brl.Book).HasPrincipalKey(b => b.Id),
                      j => j.HasKey(brl => new { brl.Book, brl.Rating })
                   );

        modelBuilder.Entity<Book>()
                   .HasMany(b => b.Languages)
                   .WithMany(a => a.Books)
                   .UsingEntity<BooksLanguagesLink>(
                       l => l.HasOne<Language>().WithMany().HasForeignKey(bll => bll.LangCode).HasPrincipalKey(a => a.Id),
                       r => r.HasOne<Book>().WithMany().HasForeignKey(bll => bll.Book).HasPrincipalKey(b => b.Id),
                      j => j.HasKey(bll => new { bll.Book, bll.LangCode })
                   );

        modelBuilder.Entity<Book>()
        .HasMany(b => b.Identifiers)
        .WithOne(i => i.BookObject)
        .HasForeignKey(i => i.Book).HasPrincipalKey(b => b.Id);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.Tags)
            .WithMany(a => a.Books)
            .UsingEntity<BooksTagsLink>(
                l => l.HasOne<Tag>().WithMany().HasForeignKey(btl => btl.Tag).HasPrincipalKey(a => a.Id),
                r => r.HasOne<Book>().WithMany().HasForeignKey(btl => btl.Book).HasPrincipalKey(b => b.Id),
               j => j.HasKey(btl => new { btl.Book, btl.Tag })
            );

        modelBuilder.Entity<Book>()
 .HasMany(b => b.Publisher)
 .WithMany(a => a.Books)
 .UsingEntity<BooksPublishersLink>(
     l => l.HasOne<Publisher>().WithMany().HasForeignKey(brl => brl.Publisher).HasPrincipalKey(a => a.Id),
     r => r.HasOne<Book>().WithMany().HasForeignKey(brl => brl.Book).HasPrincipalKey(b => b.Id),
    j => j.HasKey(brl => new { brl.Book, brl.Publisher })
 );

        // // connect to DB & retrieve customColumns;
        // var customColumnsList = this.GetCustomColumns(ConnectionString);
        // foreach (var id in customColumnsList)
        // {
        //     modelBuilder.SharedTypeEntity<GenericCustomColumn>($"custom_column_{id}", entity =>
        //     {

        //         entity.ToTable($"custom_column_{id}");

        //         entity.HasIndex(e => e.Value, $"IX_custom_column_{id}_value").IsUnique();

        //         entity.HasIndex(e => e.Value, $"custom_column_{id}_idx");

        //         entity.Property(e => e.Id).HasColumnName("id");
        //         entity.Property(e => e.Link)
        //             .HasDefaultValue("")
        //             .HasColumnName("link");
        //         entity.Property(e => e.Value)
        //             .UseCollation("NOCASE")
        //             .HasColumnName("value");

        //         entity.HasMany<GenericBooksCustomColumnLink>($"books_custom_column_{id}_link").WithOne().HasForeignKey(l => l.Value);
        //     });

        //     modelBuilder.SharedTypeEntity<GenericBooksCustomColumnLink>($"books_custom_column_{id}_link", entity =>

        //    {
        //        entity.ToTable($"books_custom_column_{id}_link");

        //        entity.HasIndex(e => new { e.Book, e.Value }, $"IX_books_custom_column_{id}_link_book_value").IsUnique();

        //        entity.HasIndex(e => e.Value, $"books_custom_column_{id}_link_aidx");

        //        entity.HasIndex(e => e.Book, $"books_custom_column_{id}_link_bidx");

        //        entity.Property(e => e.Id).HasColumnName("id");
        //        entity.Property(e => e.Book).HasColumnName("book");
        //        entity.Property(e => e.Value).HasColumnName("value");


        //    });


        //}






    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var sqlServerOptionsExtension =
                    optionsBuilder.Options.FindExtension<SqliteOptionsExtension>();
        if (sqlServerOptionsExtension != null)
        {
            this.ConnectionString = sqlServerOptionsExtension.Connection.ConnectionString;
        }
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


    private List<int> GetCustomColumns(string connectionString)
    {
        var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText =
        @"
        SELECT id
        FROM custom_columns;
    ";

        List<int> columnIds = new List<int>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                columnIds.Add(id);

                Console.WriteLine($"Hello, custom_column_{id}!");
            }
        }
        connection.Close();
        return columnIds;
    }


}



public partial class Book
{
    virtual public ICollection<Author> Authors { get; set; } =[];
    virtual public ICollection<Series> Series { get; set; } = [];
    virtual public ICollection<Rating> Rating { get; set; } = [];
    virtual public ICollection<Language> Languages { get; set; } = [];
    virtual public ICollection<Identifier> Identifiers { get; set; } = [];
    virtual public ICollection<Tag> Tags { get; set; } = [];
    virtual public ICollection<Publisher> Publisher { get; set; } = [];
    virtual public Comment? Comments {get;set;} = null;
    virtual public ICollection<Datum> Data {get;set;} = [];

    virtual public IDictionary<int, CustomColumn> CustomColumns {get;set;} = new Dictionary<int,CustomColumn>();


}

public partial class Author
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}

public partial class Series
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}


public partial class Rating
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}
public partial class Language
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}

public partial class Identifier
{
    virtual public Book BookObject { get; set; } = null!;

}

public partial class Tag
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}

public partial class Publisher
{
    virtual public ICollection<Book> Books { get; set; } = null!;
}
public partial class BooksAuthorsLink
{

    virtual public Author AuthorObject { get; set; } = null!;
    virtual public Book BookObject { get; set; } = null!;
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
public partial class BooksLanguagesLink
{
    virtual public Book BookObject { get; set; } = null!;
    virtual public Language LanguageObject { get; set; } = null!;
}

public partial class BooksTagsLink
{
    virtual public Book BookObject { get; set; } = null!;
    virtual public Tag TagObject { get; set; } = null!;
}

public partial class BooksPublishersLink
{
    virtual public Book BookObject { get; set; } = null!;
    virtual public Publisher PublisherObject { get; set; } = null!;
}

public partial class CustomColumn
{ 

    virtual public ICollection<GenericCustomColumn> Data { get; set; } = [];
}

public partial class GenericCustomColumn
{

    public int Id { get; set; }

    public string Value { get; set; } = null!;

    public string Link { get; set; } = null!;

    virtual public ICollection<GenericBooksCustomColumnLink> ColumnLink { get; set; } = null!;


    // virtual public ICollection<Book> Books { get; set; } = null!;
}

public partial class GenericBooksCustomColumnLink
{
    // virtual public Book BookObject { get; set; } = null!;
    // virtual public GenericCustomColumn CustomColumnObject { get; set; } = null!;

    public int Id { get; set; }

    public int Book { get; set; }

    public int Value { get; set; }
}

