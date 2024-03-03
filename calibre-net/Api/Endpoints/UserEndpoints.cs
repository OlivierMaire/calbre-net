using calibre_net.Services;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace calibre_net.Api.Endpoints;

public class User : Group
{
    public User()
    {
        Configure("user", ep => ep.Description(x => x.AllowAnonymous().WithGroupName("user")));
    }
}

public sealed class GetMyselfEndpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/me");
        Version(1);
        Group<User>();
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
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(PermissionStore.GetPermissions());
    }
}

public sealed class GetUserEndpoint(UserService userService) : EndpointWithoutRequest<UserModelExtended>
{
    private string Id { get; set; }
    private readonly UserService service = userService;

    public override void Configure()
    {
        Get("/{id}");
        Version(1);
        Group<User>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(await service.GetUserAsync(Id));
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
    }

    public override async Task HandleAsync(UserModelExtended req, CancellationToken ct)
    {
        await SendOkAsync(await service.UpdateUserAsync(req));
    }
}

public sealed class AddUserEndpoint(UserService userService) : Endpoint<UserModelExtended, UserModelExtended>
{
    private readonly UserService service = userService;
    public override void Configure()
    {
        Put("/add");
        Version(1);
        Group<User>();
    }

    public override async Task HandleAsync(UserModelExtended req, CancellationToken ct)
    {
        try
        {
            var user = await service.AddUserAsync(req);
            await SendOkAsync(user);
            return; 
        }
        catch (ServiceException se)
        {
            if (se.StatusCode == 400)
                await SendResultAsync(TypedResults.BadRequest(se.Errors));
            // SendBadRequestAsync(se.Errors);
            return; 
        }
        await SendResultAsync(TypedResults.BadRequest());
        // return BadRequest();
    }
}


public sealed class DeleteUserEndpoint(UserService userService) : EndpointWithoutRequest<UserModelExtended>
{
    private string Id { get; set; }
    private readonly UserService service = userService;

    public override void Configure()
    {
        Delete("/{id}");
        Version(1);
        Group<User>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await service.DeleteUser(Id, null);
        await SendOkAsync();
    }
}
