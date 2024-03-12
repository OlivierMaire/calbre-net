namespace Calibre_net.Data.Calibre;


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
