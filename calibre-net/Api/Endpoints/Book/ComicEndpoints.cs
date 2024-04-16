using System.Drawing;
using Calibre_net.Api.Endpoints;
using Calibre_net.Services;
using Calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using SkiaSharp;

public sealed class GetPageEndpoint(BookService bookService,
ConfigurationService configService, ComicService comicService,
ILogger<GetPageEndpoint> logger) : Endpoint<GetPageRequest, byte[]>
{
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;

    private readonly ComicService comicService = comicService;
    private readonly ILogger<GetPageEndpoint> _logger = logger;

    public override void Configure()
    {
        Get("/{BookId:int}/{BookFormat}/page/{PageId:int}");
        Version(1);
        Group<Book>();
        Policies(PermissionType.BOOK_VIEW);
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
                        using (stream)
                        {
                            var skData = SKData.Create(stream);
                            SKBitmap image = SKBitmap.Decode(skData);

                            var (size, leftcolor, rightcolor) = GetImageInfo(image);
                            HttpContext.Response.Headers.Append("X-FileInfo-Width", size?.Width.ToString());
                            HttpContext.Response.Headers.Append("X-FileInfo-Height", size?.Height.ToString());
                            HttpContext.Response.Headers.Append("X-FileInfo-LeftColor", leftcolor);
                            HttpContext.Response.Headers.Append("X-FileInfo-RightColor", rightcolor);

                            var newStream = skData.AsStream();

                            await SendStreamAsync(newStream, contentType: contentType ?? "image/jpg",
                            // fileName: $"{req.BookId}_{req.PageId}.jpg",
                            cancellation: ct);

                            // await stream.DisposeAsync();
                            return;
                        }
                    }

                }

            }
        }

        await SendNotFoundAsync();

    }

    private (Size?, string?, string?) GetImageInfo(SKBitmap image)
    {
        Size? ImageSize = null;
        string? LeftColor = null;
        string? RightColor = null;
        try
        {
            ImageSize = new Size(image.Width, image.Height);
            (LeftColor, RightColor) = GetMainColorFromLeftStrip(image);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "SkiaSharp Error");
        }
        return (ImageSize, LeftColor, RightColor);
    }


    private (string, string) GetMainColorFromLeftStrip(SKBitmap image)
    {
        Dictionary<SKColor, int> colorOccurrencesLeft = [];
        Dictionary<SKColor, int> colorOccurrencesRight = [];

        // Loop through the first 10 pixels from the left side of the image
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                var currentColor = image.GetPixel(x, y);

                if (colorOccurrencesLeft.ContainsKey(currentColor))
                {
                    colorOccurrencesLeft[currentColor]++;
                }
                else
                {
                    colorOccurrencesLeft[currentColor] = 1;
                }
            }

            for (int x = image.Width - 1; x >= image.Width - 10; x--)
            {
                var currentColor = image.GetPixel(x, y);

                if (colorOccurrencesRight.ContainsKey(currentColor))
                {
                    colorOccurrencesRight[currentColor]++;
                }
                else
                {
                    colorOccurrencesRight[currentColor] = 1;
                }
            }
        }

        // Find the most frequent color
        var mainColorLeft = colorOccurrencesLeft.OrderByDescending(c => c.Value).First().Key;
        var mainColorRight = colorOccurrencesRight.OrderByDescending(c => c.Value).First().Key;

        return ($"#{mainColorLeft.Red:X2}{mainColorLeft.Green:X2}{mainColorLeft.Blue:X2}", $"#{mainColorRight.Red:X2}{mainColorRight.Green:X2}{mainColorRight.Blue:X2}");
    }
}