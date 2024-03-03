
using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;

public class Permission
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}

public static class PermissionType
{
    public const string ADMIN = "Admin";
}
public static class PermissionStore
{


    public static List<Permission> GetPermissions()
    {
        return new List<Permission>()
        {
            new()
            {
                Name = PermissionType.ADMIN,
                Description = "Can access and edit Admin screens."
            },
            new()
            {
                Name = "Edit",
                Description = "Can Edit info."
            },
            new()
            {
                Name = "Delete",
                Description = "Can Delete content."
            },
            new()
            {
                Name = "Download.Allow",
                Description = "Can Download content."
            },
            new()
            {
                Name = "Viewer.Allow",
                Description = "Can use in browser content viewer."
            },
            new()
            {
                Name = "PasswordChange.Allow",
                Description = "Can change passwords of users."
            },
            new()
            {
                Name = "PublicShelf.Edit",
                Description = "Can edit public shelves."
            },
            new()
            {
                Name = "Hot.Show",
                Description = ""
            },
            new()
            {
                Name = "Downloaded.Show",
                Description = ""
            },
            new()
            {
                Name = "TopRated.Show",
                Description = ""
            },
            new()
            {
                Name = "ReadUnread.Show",
                Description = ""
            },
            new()
            {
                Name = "Random.Show",
                Description = ""
            },
            new()
            {
                Name = "Category.Show",
                Description = ""
            },
            new()
            {
                Name = "Series.Show",
                Description = ""
            },
            new()
            {
                Name = "Author.Show",
                Description = ""
            },
            new()
            {
                Name = "Publisher.Show",
                Description = ""
            },
            new()
            {
                Name = "Language.Show",
                Description = ""
            },
            new()
            {
                Name = "Ratings.Show",
                Description = ""
            },
            new()
            {
                Name = "FileFormat.Show",
                Description = ""
            },
            new()
            {
                Name = "Archived.Show",
                Description = ""
            },
            new()
            {
                Name = "List.Show",
                Description = ""
            },
            new()
            {
                Name = "RandomDetailView.Show",
                Description = ""
            }

        };
    }

    public static bool HasPermission(this UserModel user, string permission)
    {
        return user.Permissions.Contains(permission);
    }

    public static bool HasPermission(this UserModelExtended user, string permission)
    {
        // return user.Permissions.Contains(permission);
        if (user.PermissionsDictionary == null)
            return false;
        return user.PermissionsDictionary.ContainsKey(permission) ? user.PermissionsDictionary[permission] == "on" : false;
    }

}