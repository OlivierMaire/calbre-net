using Calibre_net.Services;
using Calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace Calibre_net.Api.Endpoints;

public class User : Group
{
    public User()
    {
        Configure("user", ep => ep.Description(x => x.WithGroupName("user")));
    }
}

public sealed class GetMyselfEndpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/me");
        Version(1);
        Group<User>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync("hello api world", ct);
    }
}

public sealed class GetAllUsersEndpoint(UserService userService) : EndpointWithoutRequest<List<UserModel>>
{
    private readonly UserService service = userService;

    public override void Configure()
    {
        Get("/all");
        Version(1);
        Group<User>();
        Policies(PermissionType.ADMIN_USER);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(await service.GetAllUsersAsync());

    }
}

public sealed class GetAllPermissionsEndpoint : EndpointWithoutRequest<List<Permission>>
{

    public override void Configure()
    {
        Get("/allPermissions");
        Version(1);
        Group<User>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(PermissionStore.GetPermissions());
    }
}

public sealed class GetUserEndpoint(UserService userService) : Endpoint<GetUserRequest, UserModelExtended>
{
    private readonly UserService service = userService;

    public override void Configure()
    {
        Get("/{id}");
        Version(1);
        Group<User>();
        Policies(PermissionType.ADMIN_USER);
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        await SendOkAsync(await service.GetUserAsync(req.Id));
    }
}

public sealed class UpdateUserEndpoint(UserService userService) : Endpoint<UserModelExtended, UserModelExtended>
{
    private readonly UserService service = userService;
    public override void Configure()
    {
        Post("/update");
        Version(1);
        Group<User>();
        Policies(PermissionType.ADMIN_USER);
    }

    public override async Task HandleAsync(UserModelExtended req, CancellationToken ct)
    {
        await SendOkAsync(await service.UpdateUserAsync(req));
    }
}

public sealed class AddUserEndpoint(UserService userService) : Endpoint<UserModelExtended, Results<Ok<UserModelExtended>,BadRequest<string[]>>>
{
    private readonly UserService service = userService;
    public override void Configure()
    {
        Put("/add");
        Version(1);
        Group<User>();
        Policies(PermissionType.ADMIN_USER);
    }

    public override async Task HandleAsync(UserModelExtended req, CancellationToken ct)
    {
        try
        {
            var user = await service.AddUserAsync(req);
            await SendResultAsync(TypedResults.Ok(user));
            return;
        }
        catch (ServiceException se)
        {
            if (se.StatusCode == 400)
            {
                await SendResultAsync(TypedResults.BadRequest(se.Errors));
                // SendBadRequestAsync(se.Errors);
                return;
            }
        }
        await SendResultAsync(TypedResults.BadRequest());
        // return BadRequest();
    }
}


public sealed class DeleteUserEndpoint(UserService userService) : Endpoint<DeleteUserRequest, bool>
{
    private readonly UserService service = userService;

    public override void Configure()
    {
        Delete("/{id}");
        Version(1);
        Group<User>();
        Policies(PermissionType.ADMIN_USER);
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        /* TODO set current user */ 
        await service.DeleteUser(req.Id, currentUserId);
        await SendOkAsync(true);
    }
}
