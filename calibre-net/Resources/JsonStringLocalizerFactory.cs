using calibre_net.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

[SingletonRegistration]
public class JsonStringLocalizerFactory : IStringLocalizerFactory{

private string ResourcesPath { get; }
public bool ShowKeyNameIfEmpty { get; }
public bool ShowDefaultIfEmpty { get; }
public JsonStringLocalizerFactory(
    IOptions<JsonStringLocalizerOptions> options)
{
    ResourcesPath = options.Value.ResourcesPath;
    ShowKeyNameIfEmpty = options.Value.ShowKeyNameIfEmpty;
    ShowDefaultIfEmpty = options.Value.ShowDefaultIfEmpty;
}

public IStringLocalizer Create(Type resourceSource)
{
    var resources = new EmbeddedFileProvider(
        resourceSource.Assembly);
    return new JsonStringLocalizer(
        resources,
        ResourcesPath,
        resourceSource.Name,
        ShowKeyNameIfEmpty,
        ShowDefaultIfEmpty);
}

     public IStringLocalizer Create(string baseName, string location)
        {
            throw new NotImplementedException();
        }

}