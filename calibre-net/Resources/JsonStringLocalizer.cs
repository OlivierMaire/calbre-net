using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private IFileProvider FileProvider { get; }
    private string Name { get; }
    private string ResourcesPath { get; }
    public bool ShowKeyNameIfEmpty { get; }
    public bool ShowDefaultIfEmpty { get; }

    public JsonStringLocalizer(IFileProvider fileProvider, string resourcePath, string name, bool showKeyNameIfEmpty = false, bool showDefaultIfEmpty = false)
    {
        FileProvider = fileProvider;
        Name = name;
        ResourcesPath = resourcePath;
        ShowKeyNameIfEmpty = showKeyNameIfEmpty;
        ShowDefaultIfEmpty = showDefaultIfEmpty;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        throw new NotImplementedException();
    }

    public IStringLocalizer WithCulture(CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public LocalizedString this[string name]
    {
        get
        {
            var stringMap = LoadStringMap();
            if (stringMap.ContainsKey(name))
                return new LocalizedString(name, stringMap[name]);

            if (ShowDefaultIfEmpty)
            {
                stringMap = LoadDefaultStringMap();
                if (stringMap.ContainsKey(name))
                    return new LocalizedString(name, stringMap[name]);
            }

            if (ShowKeyNameIfEmpty)
                return new LocalizedString(name, $"({name}.{CultureInfo.CurrentUICulture.Name})");

            return new LocalizedString(name, string.Empty);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
              var stringMap = LoadStringMap();
            if (stringMap.ContainsKey(name))
                return new LocalizedString(name, string.Format(stringMap[name], arguments));

            if (ShowDefaultIfEmpty)
            {
                stringMap = LoadDefaultStringMap();
                if (stringMap.ContainsKey(name))
                    return new LocalizedString(name, string.Format(stringMap[name], arguments));
            }

            if (ShowKeyNameIfEmpty)
                return new LocalizedString(name, $"({name}.{CultureInfo.CurrentUICulture.Name})");

            return new LocalizedString(name, string.Empty);
        }
    }

    private Dictionary<string, string> LoadStringMap()
    {
        var cultureInfo = CultureInfo.CurrentUICulture;
        var fileInfo =
            FileProvider.GetFileInfo(
                Path.Combine(ResourcesPath, $"{Name}_{cultureInfo.Name}.json"));
        if (!fileInfo.Exists)
        {
            fileInfo =
               FileProvider.GetFileInfo(
                   Path.Combine(ResourcesPath, $"{Name}.{cultureInfo.TwoLetterISOLanguageName}.json"));
        }
        if (!fileInfo.Exists)
        {
            fileInfo =
                FileProvider.GetFileInfo(
                    Path.Combine(ResourcesPath, $"{Name}.json"));
        }

        using var stream = fileInfo.CreateReadStream();

        return JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream).Result;
    }


    private Dictionary<string, string> LoadDefaultStringMap()
    {
        var fileInfo =
             FileProvider.GetFileInfo(
                 Path.Combine(ResourcesPath, $"{Name}.json"));

        using var stream = fileInfo.CreateReadStream();

        return JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream).Result;
    }
}