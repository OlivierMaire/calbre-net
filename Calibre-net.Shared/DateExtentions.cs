using System.Globalization;

namespace Calibre_net.Shared;

public static class DateExtensions
{
    static public string GetDatePatternWithoutWeekday(CultureInfo cultureInfo)
    {
        string[] patterns = cultureInfo.DateTimeFormat.GetAllDateTimePatterns();

        string longPattern = cultureInfo.DateTimeFormat.LongDatePattern;

        string acceptablePattern = String.Empty;

        foreach (string pattern in patterns)
        {
            if (longPattern.Contains(pattern) && !pattern.Contains("ddd") && !pattern.Contains("dddd"))
            {
                if (pattern.Length > acceptablePattern.Length)
                {
                    acceptablePattern = pattern;
                }
            }
        }

        if (String.IsNullOrEmpty(acceptablePattern))
        {
            return longPattern;
        }
        return acceptablePattern;
    }
}