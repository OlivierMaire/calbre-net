using Asp.Versioning;
using calibre_net.Services;
using Microsoft.AspNetCore.Mvc;
using calibre_net.Shared.Models;

namespace calibre_net.Api;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService userService;

    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("me")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMyself()
    {


        return Ok("hello api world");
    }


    [HttpGet(nameof(GetAllUsersAsync))]
    [ProducesResponseType(typeof(List<UserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        return Ok(await this.userService.GetAllUsersAsync());
    }

    [HttpGet(nameof(GetAllPermissions))]
    [ProducesResponseType(typeof(List<Permission>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetAllPermissions()
    {
        return Ok(PermissionStore.GetPermissions());
    }

    [HttpGet(nameof(GetUserAsync))]
    [ProducesResponseType(typeof(UserModelExtended), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserAsync(string id)
    {
        return Ok(await this.userService.GetUserAsync(id));
    }

    [HttpPost(nameof(UpdateUserAsync))]
    [ProducesResponseType(typeof(UserModelExtended), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAsync(UserModelExtended model)
    {
        return Ok(await this.userService.UpdateUserAsync(model));
    }

    [HttpPut(nameof(AddUserAsync))]
    [ProducesResponseType(typeof(UserModelExtended), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(string[]))]
    public async Task<IActionResult> AddUserAsync(UserModelExtended model)
    {
        try
        {
            var user = await this.userService.AddUserAsync(model);
            return Ok(user);
        }
        catch (ServiceException se)
        {
            if (se.StatusCode == 400)
                return BadRequest(se.Errors);
        }
        return BadRequest();
    }

    [HttpDelete(nameof(DeleteUserAsync))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserAsync(string userId)
    {
        try{
            await this.userService.DeleteUser(userId, User);
        }
        catch{
            return BadRequest();
        }
        return Ok();
    }


}