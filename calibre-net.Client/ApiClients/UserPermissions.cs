namespace calibre_net.Client.ApiClients;

public static class UserModelExtension{

      public static bool HasPermission(this UserModelExtended user, string permission)
    {
        // return user.Permissions.Contains(permission);
if (user.PermissionsDictionary == null)
    return false;
       return user.PermissionsDictionary.ContainsKey(permission) ? user.PermissionsDictionary[permission] == "on" : false;
    }

        public static bool HasPermission(this UserModel user, string permission)
    {
        return user.Permissions.Contains(permission);

    }
}
