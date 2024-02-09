using System.Security.Claims;
using calibre_net.Client.Services;
using calibre_net.Data;
using calibre_net.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using Z.EntityFramework.Plus;

namespace calibre_net.Services;

[ScopedRegistration]
public class UserService(ApplicationDbContext dbContext,
IUserStore<ApplicationUser> userStore,
UserManager<ApplicationUser> userManager,
SignInManager<ApplicationUser> signInManager,
ILogger<UserService> logger)
{
    private readonly ApplicationDbContext dbContext = dbContext;
    private readonly IUserStore<ApplicationUser> userStore = userStore;
    private readonly UserManager<ApplicationUser> userManager = userManager;
    private readonly SignInManager<ApplicationUser> signInManager = signInManager;
    private readonly ILogger<UserService> logger = logger;

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var users = await dbContext.Users.Select(u => new UserModel
        {
            Id = u.Id,
            Name = u.UserName,
            Email = u.Email,
            PreferredLocale = u.PreferredLocale,
            Permissions = u.Permissions.Select(p => p.PermissionName).ToList()
        }).ToListAsync();

        return users;
    }

    public async Task<UserModelExtended> GetUserAsync(string id)
    {
        var allPermissions = PermissionStore.GetPermissions();

        var user = await dbContext.Users.Select(u => new UserModelExtended
        {
            Id = u.Id,
            Name = u.UserName,
            Email = u.Email,
            PreferredLocale = u.PreferredLocale,
            Permissions = u.Permissions.Select(p => p.PermissionName).ToList()


        }).FirstAsync(u => u.Id == id);

        user.PermissionsDictionary = allPermissions.Select(p => new
        { Key = p.Name, Value = user.Permissions.Any(up => up == p.Name) ? "on" : "off" })
                    .AsEnumerable().ToDictionary(d => d.Key, d => d.Value);
        return user;
    }

    public async Task<UserModelExtended> UpdateUserAsync(UserModelExtended model)
    {
        var allPermissions = PermissionStore.GetPermissions();
        var user = await dbContext.Users.Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == model.Id);

        if (user == null)
            return await GetUserAsync(model.Id);

        if (user.UserName != model.Name)
            user.UserName = model.Name;
        if (user.Email != model.Email)
            user.Email = model.Email; // need to reset confirmed flag.
        if (user.PreferredLocale != model.PreferredLocale)
            user.PreferredLocale = model.PreferredLocale;

        if (model.PermissionsDictionary != null)
        {
            foreach (var p in allPermissions)
            {
                if (!model.PermissionsDictionary.ContainsKey(p.Name) || model.PermissionsDictionary[p.Name] != "on")
                {
                    var perm = user.Permissions.FirstOrDefault(up => up.PermissionName == p.Name);
                    if (perm != null)
                        user.Permissions.Remove(perm);
                }

                if (model.PermissionsDictionary.ContainsKey(p.Name) && model.PermissionsDictionary[p.Name] == "on")
                {
                    if (!user.Permissions.Any(up => up.PermissionName == p.Name))
                        user.Permissions.Add(new UserPermission()
                        {
                            UserId = user.Id,
                            PermissionName = p.Name,
                        });

                }
            }
        }


        // On Permission update Expire the tag with:
        QueryCacheManager.ExpireTag("permissions", user.Id);

        await dbContext.SaveChangesAsync();

        return await GetUserAsync(model.Id);
    }

    public async Task<UserModelExtended> AddUserAsync(UserModelExtended model)
    {

        IEnumerable<IdentityError>? identityErrors;
        var user = CreateUser();

        await userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
        user.EmailConfirmed = true;
        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            identityErrors = result.Errors;
            throw new ServiceException("Error Creating User", 400, identityErrors.Select(i => i.Description).ToArray());
        }

        logger.LogInformation("User created a new account with password.");

        var userId = await userManager.GetUserIdAsync(user);
        model.Id = userId;
        // update to set permissions
        return await UpdateUserAsync(model);
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)userStore;
    }

    public async Task DeleteUser(string userId, ClaimsPrincipal User)
    {
        var currentUser = await userManager.GetUserAsync(User);
        var user = await userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Unexpected error occurred deleting user.");
            }
            if (userId == currentUser?.Id)
            {
                await signInManager.SignOutAsync();
            }
        }
    }
}

