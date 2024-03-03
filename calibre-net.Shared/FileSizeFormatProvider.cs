namespace calibre_net.Shared;

// Define a custom format provider class
public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
{
    // Implement the GetFormat method
    public object GetFormat(Type formatType)
    {
        if (formatType == typeof(ICustomFormatter))
            return this;
        return null;
    }

    // Implement the Format method
    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        // Check if the format is valid
        if (format == null || !format.StartsWith("fs"))
        {
            // Use the default format
            return defaultFormat(format, arg, formatProvider);
        }

        // Check if the argument is a number
        if (arg is string)
        {
            // Use the default format
            return defaultFormat(format, arg, formatProvider);
        }

        // Convert the argument to a decimal value
        decimal size;
        try
        {
            size = Convert.ToDecimal(arg);
        }
        catch (InvalidCastException)
        {
            // Use the default format
            return defaultFormat(format, arg, formatProvider);
        }

        // Define the file size units
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };

        // Calculate the order of magnitude
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }

        // Get the precision from the format string
        string precision = format.Substring(2);
        if (String.IsNullOrEmpty(precision))
            precision = "2";

        // Return the formatted file size string
        return String.Format("{0:N" + precision + "} {1}", size, sizes[order]);
    }

    // A helper method for using the default format
    private static string defaultFormat(string format, object arg, IFormatProvider formatProvider)
    {
        IFormattable formattableArg = arg as IFormattable;
        if (formattableArg != null)
        {
            return formattableArg.ToString(format, formatProvider);
        }
        return arg.ToString();
    }
}

// // Use the custom format provider
// int fileSize = 123456789;
// Console.WriteLine(String.Format(new FileSizeFormatProvider(), "File size: {0:fs}", fileSize));
// // Output: File size: 117.74 MB

public static class IntExtension { 

// Define a simple method
public static string BytesToString(this int byteCount)
{
    // Define the file size units
    string[] sizes = { "B", "KB", "MB", "GB", "TB" };

    // Check if the byte count is zero
    if (byteCount == 0)
        return "0" + sizes[0];

    // Get the absolute value of the byte count
    int bytes = Math.Abs(byteCount);

    // Calculate the order of magnitude
    int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));

    // Calculate the number with the appropriate scale
    double num = Math.Round(bytes / Math.Pow(1024, place), 1);

    // Return the formatted file size string
    return (Math.Sign(byteCount) * num).ToString() + sizes[place];
}

// // Use the simple method
// int fileSize = 123456789;
// Console.WriteLine("File size: " + BytesToString(fileSize));
// // Output: File size: 117.7MB
}