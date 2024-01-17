using Asp.Versioning;
using calibre_net.Data;
using calibre_net.Services;
using calibre_net.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace calibre_net.Api;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ConfigurationController(ConfigurationService configurationService) : ControllerBase
{
    private readonly ConfigurationService configurationService = configurationService;

    [HttpGet("database")]
    [ProducesResponseType(typeof(DatabaseConfiguration), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDatabaseConfigurationAsync()
    {
        return Ok(await configurationService.GetDatabaseConfigurationAsync());
    }

    [HttpPost("database")]
    [ProducesResponseType(typeof(DatabaseConfiguration), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetDatabaseConfigurationAsync(DatabaseConfiguration model)
    {
        return Ok(await configurationService.SetDatabaseConfigurationAsync(model));
    }

}