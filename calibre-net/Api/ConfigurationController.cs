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

    // [HttpGet("database")]
    // [ProducesResponseType(typeof(DatabaseConfiguration), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetDatabaseConfigurationAsync()
    // {
    //     return Ok(await configurationService.GetDatabaseConfigurationAsync());
    // }

    // [HttpPost("database")]
    // [ProducesResponseType(typeof(DatabaseConfiguration), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> SetDatabaseConfigurationAsync(DatabaseConfiguration model)
    // {
    //     return Ok(await configurationService.SetDatabaseConfigurationAsync(model));
    // }

    // [HttpGet("basic")]
    // [ProducesResponseType(typeof(BasicConfiguration), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> GetBasicConfigurationAsync()
    // {
    //     return Ok(await configurationService.GetBasicConfigurationAsync());
    // }

    // [HttpPost("basic")]
    // [ProducesResponseType(typeof(BasicConfiguration), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> SetBasicConfigurationAsync(BasicConfiguration model)
    // {
    //     return Ok(await configurationService.SetBasicConfigurationAsync(model));
    // }

    [HttpGet("calibre")]
    [ProducesResponseType(typeof(CalibreConfiguration), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCalibreConfigurationAsync()
    {
        return Ok(await configurationService.GetCalibreConfigurationAsync());
    }

    [HttpPost("calibre")]
    [ProducesResponseType(typeof(CalibreConfiguration), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetCalibreConfigurationAsync(CalibreConfiguration model)
    {
        return Ok(await configurationService.SetCalibreConfigurationAsync(model));
    }

    [HttpGet(nameof(GetConfigurationValue))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetConfigurationValue(string key)
    {
        return Ok(await configurationService.GetConfigurationValue(key));
    }

}