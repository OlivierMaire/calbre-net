using calibre_net.Services;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.StaticFiles;
using ATL.AudioData;
using ATL;
using static ATL.PictureInfo;
using calibre_net.Data;
using Microsoft.AspNetCore.Authorization;
using FastEndpoints.Security;

namespace calibre_net.Api.Endpoints;

public class Book : Group
{
    public Book()
    {
        Configure("book", ep => ep.Description(x => x.WithGroupName("book")));
    }
}

public sealed class GetBooksEndpoint(BookService service) : EndpointWithoutRequest<List<BookDto>>
{
    private readonly BookService service = service;

    public override void Configure()
    {
        Get("/all");
        Version(1);
        Group<Book>();
        ResponseCache(60); //cache for 60 seconds
        Policies(PermissionType.BOOK_VIEW);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(service.GetBooks(new SearchRequest()), ct);
    }
}

public sealed class GetBookEndpoint(BookService service) : Endpoint<GetBookRequest, BookDto>
{
    // [JsonSchema(JsonObjectType.Int32, Format = "guid")]
    // private int Id { get; set; }
    private readonly BookService service = service;

    public override void Configure()
    {
        Get("/{Id:int}");
        Version(1);
        Group<Book>();
        ResponseCache(60); //cache for 60 seconds
        Policies(PermissionType.BOOK_VIEW);
    }

    public override async Task HandleAsync(GetBookRequest req, CancellationToken ct)
    {
        var book = service.GetBook(req.Id);
        if (book == null)
            await SendNotFoundAsync(ct);
        else
            await SendOkAsync(book);
    }
}

public sealed class GetBookCoverEndpoint(BookService bookService, ConfigurationService configService) : Endpoint<GetBookRequest, byte[]>
{
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;
    private const int _7DaysInSeconds = 86400;

    public override void Configure()
    {
        Get("/cover/{Id:int}");
        Version(1);
        Group<Book>();
        ResponseCache(_7DaysInSeconds); //cache for 7 days
        Policies(PermissionType.BOOK_VIEW);

    }

    public override async Task HandleAsync(GetBookRequest req, CancellationToken ct)
    {
        var conf = configService.GetCalibreConfiguration();

        // get book
        var bookPath = bookService.GetBookCover(req.Id);
        if (!String.IsNullOrEmpty(bookPath))
        {
            var path = conf.Database?.Location ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += bookPath;

            if (System.IO.File.Exists(path))
            {
                new FileExtensionContentTypeProvider()
                    .TryGetContentType(path, out var contentType);
                var stream = System.IO.File.OpenRead(path);
                await SendStreamAsync(stream, contentType: contentType ?? "image/jpg",
                cancellation: ct, fileName: $"{req.Id}.jpg");
                return;
            }

        }
        await SendErrorsAsync();
    }
}


public sealed class DownloadBookEndpoint(BookService bookService, ConfigurationService configService) : Endpoint<DownloadBookRequest, byte[]>
{
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;
    private const int _7DaysInSeconds = 86400;

    public override void Configure()
    {
        Get("/download/{Id:int}/{format}");
        Version(1);
        Group<Book>();
        ResponseCache(_7DaysInSeconds); //cache for 7 days
        Policies(PermissionType.BOOK_VIEW);

    }

    public override async Task HandleAsync(DownloadBookRequest req, CancellationToken ct)
    {
        var conf = configService.GetCalibreConfiguration();

        // get book
        var bookPath = bookService.GetBookFile(req.Id, req.Format);
        if (!String.IsNullOrEmpty(bookPath))
        {
            var path = conf.Database?.Location ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += bookPath;
            if (System.IO.File.Exists(path))
            {
                new FileExtensionContentTypeProvider()
                .TryGetContentType(path, out var contentType);
                // FileInfo fi = new FileInfo(path);
                // await SendFileAsync(fi, contentType:  contentType ?? "application/octet-stream", enableRangeProcessing: true);

                this.HttpContext.Response.Headers.ContentDisposition = $"inline;"; // filename={Path.GetFileName(path)}";
                var stream = System.IO.File.OpenRead(path);
                await SendStreamAsync(stream, contentType: contentType ?? "application/octet-stream",
                cancellation: ct
                //, fileName: Path.GetFileName(path) // do not set filename else the Content-Disposition Header becomes attachment;
                , enableRangeProcessing: true);
                return;
            }

        }
        await SendErrorsAsync();
    }
}

public sealed class SetBookmarkEndpoint(ApplicationDbContext dbContext) : Endpoint<SetBookmarkRequest, Ok>
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public override void Configure()
    {
        Get("/setBookmark");
        Version(1);
        Group<Book>();
        Policies(PermissionType.BOOK_VIEW);
    }

    public override async Task HandleAsync(SetBookmarkRequest req, CancellationToken ct)
    {
        //   var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId");

        // save bookmark
        var bookmark = await dbContext.Bookmarks.FindAsync("720e80fb-6004-4ab8-bc5c-4675446323d9",
        (uint)req.BookId, req.BookFormat, ct);
        if (bookmark != null)
            bookmark.Position = req.Position;
        else
        {
            dbContext.Bookmarks.Add(new Bookmark
            {
                BookId = (uint)req.BookId,
                Format = req.BookFormat,
                Position = req.Position,
                UserId = "720e80fb-6004-4ab8-bc5c-4675446323d9"
            });
        }
        await dbContext.SaveChangesAsync();
        await SendOkAsync();
    }
}

public sealed class GetBookmarkEndpoint(ApplicationDbContext dbContext) : Endpoint<GetBookmarkRequest, GetBookmarkResponse>
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public override void Configure()
    {
        Get("/getBookmark");
        Version(1);
        Group<Book>();
        Policies(PermissionType.BOOK_VIEW);
    }

    public override async Task HandleAsync(GetBookmarkRequest req, CancellationToken ct)
    {
        // get bookmark
        var bookmark = await dbContext.Bookmarks.FindAsync("720e80fb-6004-4ab8-bc5c-4675446323d9", (uint)req.BookId, req.BookFormat, ct);

        if (bookmark != null)
            await SendOkAsync(new GetBookmarkResponse((int)bookmark.BookId, bookmark.Format, bookmark.Position));
        else
            await SendOkAsync(new GetBookmarkResponse((int)req.BookId, req.BookFormat, ""));
    }
}


public sealed class SearchBooksEndpoint(BookService service) : Endpoint<SearchRequest, List<BookDto>>
{
    private readonly BookService service = service;

    public override void Configure()
    {
        Post("/search");
        Version(1);
        Group<Book>();
        ResponseCache(60); //cache for 60 seconds
        Policies(PermissionType.BOOK_VIEW);
    }

    public override async Task HandleAsync(SearchRequest req, CancellationToken ct)
    {
        await SendOkAsync(service.GetBooks(req), ct);
    }
}
