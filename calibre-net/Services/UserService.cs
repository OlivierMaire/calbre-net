using calibre_net.Data;
using calibre_net.Models;

namespace calibre_net.Services;

[ScopedRegistration]
public class UserService{
    private readonly ApplicationDbContext dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public List<UserModel> GetAllUsers()
    {
        var users = dbContext.Users.Select(u => new UserModel{

        }).ToList();

        return users;
    }




}