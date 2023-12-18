using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace calibre_net.Shared.Resources;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{

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
        // var resources = new EmbeddedFileProvider(resourceSource.Assembly);
        var resources = new EmbeddedFileProvider(this.GetType().Assembly); // getting myself assembly to consolidate all file in this project.
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