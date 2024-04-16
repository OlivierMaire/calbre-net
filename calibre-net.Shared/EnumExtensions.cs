using System.Runtime.Serialization;
using Calibre_net.Shared.Contracts;

namespace Calibre_net.Shared;

public static class EnumExtensions
{
    public static string ToEnumString<T>(this T type) where T : System.Enum
    {
        var enumType = typeof(T);
        if (enumType == null)
            return string.Empty;

        var name = Enum.GetName(enumType, type);
        if (name == null)
            return string.Empty;

        var field = enumType.GetField(name);
        if (field == null)
            return string.Empty;

        var enumMemberAttribute = ((EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
        return enumMemberAttribute.Value ?? string.Empty;
    }

    public static string ToEnumOperatorString<T>(this T type) where T : System.Enum
    {
        var enumType = typeof(T);
        if (enumType == null)
            return string.Empty;

        var name = Enum.GetName(enumType, type);
        if (name == null)
            return string.Empty;

        var field = enumType.GetField(name);
        if (field == null)
            return string.Empty;

        var enumOperatorAttribute = ((EnumOperatorAttribute[])field.GetCustomAttributes(typeof(EnumOperatorAttribute), true)).Single();
        return enumOperatorAttribute.Value ?? string.Empty;
    }


    public static T? ToEnum<T>(this string str)
    {
        var enumType = typeof(T);
        foreach (var name in Enum.GetNames(enumType))
        {
            var field = enumType.GetField(name);
            if (field == null)
                return default;
            var enumMemberAttribute = ((EnumMemberAttribute[])field.GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
        }
        //throw exception or whatever handling you want or
        return default;
    }
}