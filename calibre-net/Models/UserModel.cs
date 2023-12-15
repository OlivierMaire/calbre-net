
namespace calibre_net.Models;

public class UserModel
{
    public string Id { get; internal set; }
    public string? Name { get; internal set; }
    public string? Email { get; internal set; }
    public List<string> Permissions { get; internal set; } = [];
}