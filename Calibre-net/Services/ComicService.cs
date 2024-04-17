using System.IO.Compression;
using Calibre_net.Client.Services;
using Calibre_net.Data.Calibre;
using ComicMeta;
using Calibre_net.Shared.Contracts;
using Dapper;
using Microsoft.AspNetCore.StaticFiles;

namespace Calibre_net.Services;

[ScopedRegistration]
public class ComicService(CalibreDbDapperContext dbContext, ILogger<ComicService> logger)

{
    private readonly CalibreDbDapperContext dbContext = dbContext;
    private readonly ILogger<ComicService> logger = logger;

    public ComicMeta.Metadata.GenericMetadata? GetComicInfo(string filePath)
    {
        var ca = new ComicArchive(filePath);
        if (ca.IsComicArchive)
        {
            var read_metadata = ca.ReadMetadata();

            return read_metadata;
        }
        return null;
    }

    public bool IsComicArchive(string filePath)
    {
        var ca = new ComicArchive(filePath);
        return ca.IsComicArchive;
    }

    public (Stream?, string?) GetPage(string filePath, int pageId)
    {
        var ca = new ComicArchive(filePath);
        var metadata = ca.ReadMetadata();

        var page = metadata.Pages.FirstOrDefault(p => p.PageNumber == pageId);
        if (page == null)
            return (null, null);

        new FileExtensionContentTypeProvider()
                .TryGetContentType(page.Key, out var contentType);

        return (ca.GetPageAsStream(page.Key), contentType);
    }

}