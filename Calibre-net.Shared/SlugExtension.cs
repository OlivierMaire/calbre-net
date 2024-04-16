using System.Text.RegularExpressions;

namespace Calibre_net.Shared;

public static partial class SlugExtension
{
    public static string GenerateSlug(this string phrase, int maxLength = 45)
    {
        string str = phrase.RemoveAccent().ToLower();

        str = InvalidCharsRegex().Replace(str, ""); // invalid chars           
        str = SpacesRegex().Replace(str, " ").Trim(); // convert multiple spaces into one space   
        str = str[..Math.Min(str.Length, maxLength)].Trim(); // cut and trim it   
        str = HyphensRegex().Replace(str, "-"); // hyphens   

        return str;
    }

    public static string RemoveAccent(this string txt)
    {
        byte[] bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(txt);
        return System.Text.Encoding.ASCII.GetString(bytes);
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex InvalidCharsRegex();
    [GeneratedRegex(@"\s+")]
    private static partial Regex SpacesRegex();
    [GeneratedRegex(@"\s")]
    private static partial Regex HyphensRegex();
}