using calibre_net.Api.Endpoints;
using calibre_net.Services;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

public sealed class GetPageEndpoint(BookService bookService, ConfigurationService configService, ComicService comicService) : Endpoint<GetPageRequest, byte[]>
{
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;

    private readonly ComicService comicService = comicService;

    public override void Configure()
    {
        Get("/{BookId:int}/{BookFormat}/page/{PageId:int}");
        Version(1);
        Group<Book>();
    }

    public override async Task HandleAsync(GetPageRequest req, CancellationToken ct)
    {
        var conf = configService.GetCalibreConfiguration();

        // get book
        var bookPath = bookService.GetBookFile(req.BookId, req.BookFormat);
        if (!String.IsNullOrEmpty(bookPath))
        {
            var path = conf.Database?.Location ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += bookPath;
            if (System.IO.File.Exists(path))
            {

                if (comicService.IsComicArchive(path))
                {
                    (Stream? stream, string? contentType) = comicService.GetPage(path, req.PageId);
                    if (stream != null)
                    {
                        await SendStreamAsync(stream, contentType: contentType ?? "image/jpg",
                        cancellation: ct);

                        await stream.DisposeAsync();
                        return;
                    }

                }

            }
        }

        await SendNotFoundAsync();

    }
}