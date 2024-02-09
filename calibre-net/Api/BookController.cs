using Asp.Versioning;
using calibre_net.Data;
using calibre_net.Services;
using calibre_net.Shared.Models;
using Calibre_net.Data.Calibre;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calibre_net.Api;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookController(BookService service) : ControllerBase
{
    private readonly BookService service = service;
 
    [HttpGet("books")]
    [ProducesResponseType(typeof(List<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBooks()
    {
        return Ok(service.GetBooks());
    }
 

}