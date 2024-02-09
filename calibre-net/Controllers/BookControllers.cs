
using calibre_net.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[Route("[controller]/[action]")]
public class BookController(BookService bookService, ConfigurationService configService) : Controller
{
    private const int _7DaysInSeconds = 86400;
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;

    [ActionName("cover")]
    [Route("{id}")]
    [ResponseCache( Duration = _7DaysInSeconds)]
    public IActionResult GetCover(int id)
    {
        var conf = configService.GetCalibreConfiguration();

        // get book
        var book = bookService.GetBook(id);
        if (book != null)
        {
            var path = conf.Database?.Location ?? string.Empty;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            path += book.Path;
            path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
            if (System.IO.File.Exists($"{path}cover.jpg"))
            {
                var stream = System.IO.File.OpenRead($"{path}cover.jpg");
                return new FileStreamResult(stream, "image/jpg");
            }
        }
        return BadRequest();

    }
}