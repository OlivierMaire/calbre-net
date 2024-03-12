using System.IO.Compression;
using calibre_net.Client.Services;
using calibre_net.Data.Calibre;
using ComicMeta;
using calibre_net.Shared.Contracts;
using Calibre_net.Data.Calibre;
using Dapper;

namespace calibre_net.Services;

[ScopedRegistration]
public class ComicService(CalibreDbDapperContext dbContext, ILogger<ComicService> logger)

{
    private readonly CalibreDbDapperContext dbContext = dbContext;
    private readonly ILogger<ComicService> logger = logger;

    public object GetComicInfo(string filePath)
    {
        logger.LogInformation($"Get info for {filePath}");
        var ca = new ComicArchive(filePath);
        if (ca.IsComicArchive)
        {
            ComicArchive.MetadataStyle style;
            if (ca.HasMetadata(ComicArchive.MetadataStyle.CIX))
                style = ComicArchive.MetadataStyle.CIX;
            if (ca.HasMetadata(ComicArchive.MetadataStyle.CBI))
                style = ComicArchive.MetadataStyle.CBI;
            if (ca.HasMetadata(ComicArchive.MetadataStyle.COMET))
                style = ComicArchive.MetadataStyle.COMET;

        }

        return new
        {
            fileName = filePath,
            isComicArchive = ca.IsComicArchive,
            isCIX = ca.HasMetadata(ComicArchive.MetadataStyle.CIX),
            isCBI = ca.HasMetadata(ComicArchive.MetadataStyle.CBI),
            isCOMET = ca.HasMetadata(ComicArchive.MetadataStyle.COMET),
        };
    }


}