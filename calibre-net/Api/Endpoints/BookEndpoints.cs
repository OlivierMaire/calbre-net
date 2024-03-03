using calibre_net.Services;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.StaticFiles;

namespace calibre_net.Api.Endpoints;

public class Book : Group
{
    public Book()
    {
        Configure("book", ep => ep.Description(x => x.AllowAnonymous().WithGroupName("book")));
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
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(service.GetBooks(), ct);
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

public sealed class GetBookCoverEndpoint(BookService bookService, ConfigurationService configService) : Endpoint<GetBookRequest, BookDto>
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
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            if (System.IO.File.Exists($"{path}cover.jpg"))
            {
                new FileExtensionContentTypeProvider()
             .TryGetContentType(path, out var contentType);
                // FileInfo fi = new FileInfo($"{path}cover.jpg");
                // await SendFileAsync(fi, contentType: contentType ?? "image/jpg");
                var stream = System.IO.File.OpenRead($"{path}cover.jpg");
                // return new FileStreamResult(stream, "image/jpg");
                await SendStreamAsync(stream, contentType: contentType ?? "image/jpg",
                cancellation: ct, fileName: $"{req.Id}.jpg");
                return;
            }

        }
        await SendErrorsAsync();
    }
}


public sealed class DownloadBookEndpoint(BookService bookService, ConfigurationService configService) : Endpoint<DownloadBookRequest, BookDto>
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
                // FileInfo fi = new FileInfo($"{path}cover.jpg");
                // await SendFileAsync(fi, contentType: contentType ?? "image/jpg");
                var stream = System.IO.File.OpenRead(path);
                // return new FileStreamResult(stream, "image/jpg");
                await SendStreamAsync(stream, contentType: contentType ?? "application/octet-stream",
                cancellation: ct, fileName: Path.GetFileName(path));
                return;
            }

        }
        await SendErrorsAsync();
    }
}