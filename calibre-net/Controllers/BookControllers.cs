
using calibre_net.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.StaticFiles;

[Route("[controller]/[action]")]
public class BookController(BookService bookService, ConfigurationService configService) : Controller
{
    private const int _7DaysInSeconds = 86400;
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;

    [ActionName("cover")]
    [Route("{id}")]
    [ResponseCache(Duration = _7DaysInSeconds)]
    public IActionResult GetCover(int id)
    {
        var conf = configService.GetCalibreConfiguration();

        // get book
        var bookPath = bookService.GetBookCover(id);
        if (!String.IsNullOrEmpty(bookPath))
        {
            var path = conf.Database?.Location ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += bookPath;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            if (System.IO.File.Exists($"{path}cover.jpg"))
            {
                var stream = System.IO.File.OpenRead($"{path}cover.jpg");
                return new FileStreamResult(stream, "image/jpg");
            }

        }
        return BadRequest();

    }

    [ActionName("download")]
    [Route("{id}/{format}")]
    public IActionResult DownloadFile(int id,string format)
    {
        var conf = configService.GetCalibreConfiguration();

        // get book
        var bookPath = bookService.GetBookFile(id, format);
        if (!String.IsNullOrEmpty(bookPath))
        {
            var path = conf.Database?.Location ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += bookPath;
            if (System.IO.File.Exists(path))
            {
                var stream = System.IO.File.OpenRead(path);
               new FileExtensionContentTypeProvider()
               .TryGetContentType(path, out var contentType);
                var result =  new FileStreamResult(stream,  contentType ?? "application/octet-stream");
                result.FileDownloadName = Path.GetFileName(path);
                return result;
            }   

        }
        return BadRequest();

    }

}