using calibre_net.Data;
using calibre_net.Models;
using Microsoft.EntityFrameworkCore;

namespace calibre_net.Services;

[ScopedRegistration]
public class UserService(ApplicationDbContext dbContext)
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var users = await dbContext.Users.Select(u => new UserModel{
            Id = u.Id, 
            Name = u.UserName, 
            Email = u.Email,
            Permissions = u.Permissions.Select(p => p.PermissionName).ToList()
        }).ToListAsync();

        return users;
    }

}

