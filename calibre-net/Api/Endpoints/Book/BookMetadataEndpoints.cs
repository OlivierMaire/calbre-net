using ATL;
using ATL.AudioData;
using calibre_net.Services;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using VersOne.Epub.Schema;
using static ATL.PictureInfo;
namespace calibre_net.Api.Endpoints;

public sealed class MetadataBookEndpoint(BookService bookService, ConfigurationService configService
, ComicService comicService) : Endpoint<DownloadBookRequest,
Results<Ok<GetMetadataResponse>, NotFound>>
{
    private readonly BookService bookService = bookService;
    private readonly ConfigurationService configService = configService;
    private readonly ComicService comicService = comicService;
    private const int _7DaysInSeconds = 86400;

    private static readonly string[] AUDIO_FORMATS = ["MP3", "MP4", "M4B", "M4A"];
    private static readonly string[] BOOK_FORMATS = ["AZW3", "EPUB"];
    private static readonly string[] COMICS_FORMATS = ["CBZ", "CBR", "PDF"];


    public override void Configure()
    {
        Get("/metadata/{Id:int}/{format}");
        Version(1);
        Group<Book>();
        ResponseCache(_7DaysInSeconds); //cache for 7 days
        Policies(PermissionType.BOOK_VIEW);
    }

    public override async Task<Results<Ok<GetMetadataResponse>, NotFound>> ExecuteAsync(DownloadBookRequest req, CancellationToken ct)
    {

        // Audio Metadata
        if (AUDIO_FORMATS.Contains(req.Format.ToUpper()))
        {
            var meta = GetAudioMetadata(req);
            if (meta != null)
                return TypedResults.Ok(new GetMetadataResponse(meta.ToDto()));
        }
        // Comics Metadata
        if (COMICS_FORMATS.Contains(req.Format.ToUpper()))
        {
            var meta = GetComicsMeta(req);
            if (meta != null)
                return TypedResults.Ok(new GetMetadataResponse(meta.ToDto()));

        }
        // Epub Metadata
        if (BOOK_FORMATS.Contains(req.Format.ToUpper()))
        {
            var meta = GetEpubMeta(req);
            if (meta != null)
                return TypedResults.Ok(new GetMetadataResponse(meta));

        }


        return await Task.FromResult(TypedResults.NotFound());
    }

    private EPubMetadata? GetEpubMeta(DownloadBookRequest req)
    {

        // Get Calibre data  
        var book = bookService.GetBook(req.Id);
        if (book != null)
        {
            var meta = new Shared.Contracts.EPubMetadata
            {
                Title = book.Title,
                Authors = book.Authors.ToArray()
            };
            return meta;
        }

        return null;
    }

private AudioPlayerBlazor.AudioMetadata? GetAudioMetadata(DownloadBookRequest req)
{
    var conf = configService.GetCalibreConfiguration();

    // get book
    var bookPath = bookService.GetBookFile(req.Id, req.Format);
    if (!String.IsNullOrEmpty(bookPath))
    {
        var path = conf.Database?.Location ?? string.Empty;
        path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
        path += bookPath;
        if (System.IO.File.Exists(path))
        {
            Track audioTrack = new Track(path);
            // validate picture
            if (audioTrack.EmbeddedPictures.Count == 0
            || !audioTrack.EmbeddedPictures.Any(p => p.PicType == PIC_TYPE.Generic || p.PicType == PIC_TYPE.Front))
            {
                var coverPath = bookService.GetBookCover(req.Id);
                path = conf.Database?.Location ?? string.Empty;
                path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
                path += coverPath;

                if (System.IO.File.Exists(path))
                {
                    var stream = System.IO.File.OpenRead(path);
                    audioTrack.EmbeddedPictures.Add(PictureInfo.fromBinaryData(stream, (int)stream.Length,
                     PIC_TYPE.Generic, MetaDataIOFactory.TagType.ANY, 0));
                }
            }

            var meta = audioTrack.ToAudioMetadata();
            return meta;
        }
    }
    return null;
}

private ComicMeta.Metadata.GenericMetadata? GetComicsMeta(DownloadBookRequest req)
{
    var conf = configService.GetCalibreConfiguration();

    // get book
    var bookPath = bookService.GetBookFile(req.Id, req.Format);
    if (!String.IsNullOrEmpty(bookPath))
    {
        var path = conf.Database?.Location ?? string.Empty;
        path = path.EndsWith(Path.DirectorySeparatorChar) ? path : path + Path.DirectorySeparatorChar;
        path += bookPath;
        if (System.IO.File.Exists(path))
        {
            var comic = comicService.GetComicInfo(path);
            if (comic != null)
            {
                // Get Calibre data and add missing data
                var book = bookService.GetBook(req.Id);
                if (book != null)
                {
                    // if (String.IsNullOrEmpty(comic.Title))
                    comic.Title = book.Title; // always override
                    if (String.IsNullOrEmpty(comic.Series))
                        comic.Series = book.Series.Name;
                    if (comic.Writers == null || comic.Writers.Length == 0)
                        comic.Writers = book.Authors.Select(a => a.Name).ToArray();
                    if (String.IsNullOrEmpty(comic.CommunityRating))
                        comic.CommunityRating = (book.Rating.Rating / 2.0m).ToString();
                    if (comic.Tags == null || comic.Tags.Length == 0)
                        comic.Tags = book.Tags.Select(t => t.Name).ToArray();
                    if (String.IsNullOrEmpty(comic.Summary))
                        comic.Summary = book.Comments.Text;
                }
            }
            return comic;
        }
    }

    return null;
}



}