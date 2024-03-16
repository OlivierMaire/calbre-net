using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;


public class GetMetadataResponse
{

    public object Metadata => ComicMetadata == null ? AudioMetadata! : ComicMetadata!;

    public AudioMetadata? AudioMetadata {get;set;}
    public ComicMetadata? ComicMetadata {get;set;}
}
 
 
public record AudioMetadata
{
    //
    // Summary:
    //     Title
    public string Title { get; set; } = string.Empty;

    //
    // Summary:
    //     Artist
    public string Artist { get; init; } = string.Empty;

    //
    // Summary:
    //     Composer
    public string Composer { get; init; } = string.Empty;

    //
    // Summary:
    //     Comments
    public string Comment { get; init; } = string.Empty;

    //
    // Summary:
    //     Genre
    public string Genre { get; init; } = string.Empty;

    //
    // Summary:
    //     Title of the album
    public string Album { get; init; } = string.Empty;

    //
    // Summary:
    //     Title of the original album
    public string OriginalAlbum { get; init; } = string.Empty;

    //
    // Summary:
    //     Original artist
    public string OriginalArtist { get; init; } = string.Empty;

    //
    // Summary:
    //     Copyright
    public string Copyright { get; init; } = string.Empty;

    //
    // Summary:
    //     General description
    public string Description { get; init; } = string.Empty;

    //
    // Summary:
    //     Publisher
    public string Publisher { get; init; } = string.Empty;

    //
    // Summary:
    //     Publishing date (set to DateTime.MinValue to remove)
    public DateTime? PublishingDate { get; init; }

    //
    // Summary:
    //     Album Artist
    public string AlbumArtist { get; init; } = string.Empty;

    //
    // Summary:
    //     Conductor
    public string Conductor { get; init; } = string.Empty;

    //
    // Summary:
    //     Lyricist
    public string Lyricist { get; init; } = string.Empty;

    //
    // Summary:
    //     Mapping between functions (e.g. "producer") and names. Every odd field is a function
    //     and every even is an name or a comma delimited list of names
    public string InvolvedPeople { get; init; } = string.Empty;

    //
    // Summary:
    //     Product ID
    public string ProductId { get; init; } = string.Empty;

    //
    // Summary:
    //     International Standard Recording Code (ISRC)
    public string ISRC { get; init; } = string.Empty;

    //
    // Summary:
    //     Catalog number
    public string CatalogNumber { get; init; } = string.Empty;

    //
    // Summary:
    //     Audio source URL
    public string AudioSourceUrl { get; set; } = string.Empty;

    //
    // Summary:
    //     Title sort order A string which should be used instead of the album for sorting
    //     purposes
    public string SortAlbum { get; init; } = string.Empty;

    //
    // Summary:
    //     Title sort order A string which should be used instead of the album artist for
    //     sorting purposes
    public string SortAlbumArtist { get; init; } = string.Empty;

    //
    // Summary:
    //     Title sort order A string which should be used instead of the artist for sorting
    //     purposes
    public string SortArtist { get; init; } = string.Empty;

    //
    // Summary:
    //     Title sort order A string which should be used instead of the title for sorting
    //     purposes
    public string SortTitle { get; init; } = string.Empty;

    //
    // Summary:
    //     Content group description Used if the sound belongs to a larger category of sounds/music.
    //     For example, classical music is often sorted in different musical sections (e.g.
    //     "Piano Concerto").
    public string Group { get; init; } = string.Empty;

    //
    // Summary:
    //     Series title / Movement name
    public string SeriesTitle { get; init; } = string.Empty;

    //
    // Summary:
    //     Series part / Movement index
    public string SeriesPart { get; init; } = string.Empty;

    //
    // Summary:
    //     Long description (may also be called "Podcast description")
    public string LongDescription { get; init; } = string.Empty;

    //
    // Summary:
    //     Language
    public string Language { get; init; } = string.Empty;

    //
    // Summary:
    //     Beats per minute
    public int? BPM { get; init; }

    //
    // Summary:
    //     Person or organization that encoded the file
    public string EncodedBy { get; init; } = string.Empty;

    //
    // Summary:
    //     Software that encoded the file, with relevant settings if any
    public string Encoder { get; init; } = string.Empty;

    //
    // Summary:
    //     Recording date (set to DateTime.MinValue to remove)
    public DateTime? Date { get; init; }

    //
    // Summary:
    //     Recording year
    public int? Year { get; init; }

    //
    // Summary:
    //     Original release date (set to DateTime.MinValue to remove)
    public DateTime? OriginalReleaseDate { get; init; }


    public string MimeType { get; set; } = string.Empty;

    //
    // Summary:
    //     Original release year
    public int? OriginalReleaseYear { get; init; }

    //
    // Summary:
    //     Track number
    public int? TrackNumber { get; init; }

    //
    // Summary:
    //     Total track number
    public int? TrackTotal { get; init; }

    //
    // Summary:
    //     Disc number
    public int? DiscNumber { get; init; }

    //
    // Summary:
    //     Total disc number
    public int? DiscTotal { get; init; }

    //
    // Summary:
    //     Popularity (0% = 0 stars to 100% = 5 stars) e.g. 3.5 stars = 70%
    public float? Popularity { get; init; }

    //
    // Summary:
    //     Chapters table of content description
    public string ChaptersTableDescription { get; init; } = string.Empty;

    //
    // Summary:
    //     Contains any other metadata field that is not represented by a getter in the
    //     above interface
    public IList<ChapterInfoDto> Chapters { get; init; } = [];

    //
    // Summary:
    //     Contains any other metadata field that is not represented by a getter in the
    //     above interface Use MetaDataHolder.DATETIME_PREFIX + DateTime.ToFileTime() to
    //     set dates. ATL will format them properly.
    public IDictionary<string, string> AdditionalFields { get; init; }= new Dictionary<string, string>();

    //
    // Summary:
    //     Format of the tagging systems
    // public IList<Format> MetadataFormats { get; internal set; }

    //
    // Summary:
    //     Bitrate (kilobytes per second)
    public int Bitrate { get; set; }

    //
    // Summary:
    //     Bit depth (bits per sample) -1 if bit depth is not relevant to that audio format
    public int BitDepth { get; set; }

    //
    // Summary:
    //     Sample rate (Hz)
    public double SampleRate { get; set; }

    //
    // Summary:
    //     Returns true if the bitrate is variable; false if not
    public bool IsVBR { get; set; }

    //
    // Summary:
    //     Family of the audio codec (See AudioDataIOFactory) 0=Streamed, lossy data 1=Streamed,
    //     lossless data 2=Sequenced with embedded sound library 3=Sequenced with codec
    //     or hardware-dependent sound library
    public int CodecFamily { get; set; }

    //
    // Summary:
    //     Format of the audio data
    public FormatDto AudioFormat { get;  set; } = null!;

    //
    // Summary:
    //     Duration (seconds)
    public int Duration => (int)Math.Round(DurationMs / 1000.0);

    //
    // Summary:
    //     Duration (milliseconds)
    public double DurationMs { get; set; }

    //
    // Summary:
    //     Channels arrangement
    // public ChannelsArrangements.ChannelsArrangement ChannelsArrangement { get; internal set; }

    //
    // Summary:
    //     Low-level / technical informations about the audio file
    // public TechnicalInfo TechnicalInformation { get; internal set; }

    //
    // Summary:
    //     List of pictures stored in the tag NB1 : PictureInfo.PictureData (raw binary
    //     picture data) is valued NB2 : Also allows to value embedded pictures inside chapters
    public IList<PictureInfoDto> EmbeddedPictures { get; set; } = [];

}

public record FormatDto{
 

    //
    // Summary:
    //     Name
    public string Name { get; set; } = string.Empty;

    //
    // Summary:
    //     Short name
    public string ShortName { get; set; } = string.Empty;

    //
    // Summary:
    //     Internal unique ID
    public int ID { get; set; }

   
    //
    // Summary:
    //     MIME types associated with the format
    public ICollection<string> MimeList { get; init; } = [];

    //
    // Summary:
    //     True if the format is readable by ATL
    public bool Readable { get; set; }

    //
    // Summary:
    //     True if the format is writable by ATL
    public bool Writable { get; set; }

}

public record ChapterInfoDto
{
    //
    // Summary:
    //     Start time (ms) NB : Only used when ATL.ChapterInfo.UseOffset is false
    public uint StartTime { get; init; }

    //
    // Summary:
    //     End time (ms) NB : Only used when ATL.ChapterInfo.UseOffset is false
    public uint EndTime { get; init; }

    //
    // Summary:
    //     Start offset (bytes) NB1 : Only used when ATL.ChapterInfo.UseOffset is true NB2
    //     : Only supported by ID3v2
    public uint StartOffset { get; init; }

    //
    // Summary:
    //     End offset (bytes) NB1 : Only used when ATL.ChapterInfo.UseOffset is true NB2
    //     : Only supported by ID3v2
    public uint EndOffset { get; init; }

    //
    // Summary:
    //     True to use StartOffset / EndOffset instead of StartTime / EndTime NB : Only
    //     supported by ID3v2 Default : false
    public bool UseOffset { get; init; }

    //
    // Summary:
    //     Title
    public string Title { get; init; } = string.Empty;

    //
    // Summary:
    //     Unique ID ID3v2 : Unique ID Vorbis : Chapter index (0,1,2...)
    public string UniqueID { get; init; } = string.Empty;

    //
    // Summary:
    //     Subtitle NB : Only supported by ID3v2
    public string Subtitle { get; init; } = string.Empty;

    //
    // Summary:
    //     Associated URL NB : Only supported by ID3v2
    public UrlInfoDto? Url { get; init; }
    //
    // Summary:
    //     Associated picture NB : Only supported by ID3v2 and MP4/M4A
    public PictureInfoDto? Picture { get; init; }

}

public record UrlInfoDto
{
    //
    // Summary:
    //     Description
    public string Description { get; init; } = string.Empty;

    //
    // Summary:
    //     The URL itself
    public string Url { get; init; } = string.Empty;


}

public record PictureInfoDto
{

    public enum PIC_TYPE
    {
        //
        // Summary:
        //     Unsupported (i.e. none of the supported values in the enum)
        Unsupported = 99,
        //
        // Summary:
        //     Generic
        Generic = 1,
        //
        // Summary:
        //     Front cover
        Front = 2,
        //
        // Summary:
        //     Back cover
        Back = 3,
        //
        // Summary:
        //     Media (e.g. label side of CD)
        CD = 4,
        //
        // Summary:
        //     File icon
        Icon = 5,
        //
        // Summary:
        //     Leaflet
        Leaflet = 6,
        //
        // Summary:
        //     Lead artist/lead performer/soloist
        LeadArtist = 7,
        //
        // Summary:
        //     Artist/performer
        Artist = 8,
        //
        // Summary:
        //     Conductor
        Conductor = 9,
        //
        // Summary:
        //     Band/Orchestra
        Band = 10,
        //
        // Summary:
        //     Composer
        Composer = 11,
        //
        // Summary:
        //     Lyricist/text writer
        Lyricist = 12,
        //
        // Summary:
        //     Recording location
        RecordingLocation = 13,
        //
        // Summary:
        //     During recording
        DuringRecording = 14,
        //
        // Summary:
        //     During performance
        DuringPerformance = 15,
        //
        // Summary:
        //     Movie/video screen capture
        MovieCapture = 16,
        //
        // Summary:
        //     A bright, coloured fish
        Fishie = 17,
        //
        // Summary:
        //     Illustration
        Illustration = 18,
        //
        // Summary:
        //     Band/artist logotype
        BandLogo = 19,
        //
        // Summary:
        //     Publisher/Studio logotype
        PublisherLogo = 20
    }



    //
    // Summary:
    //     Normalized picture type (see enum)
    public PIC_TYPE PicType { get; set; }


    //
    // Summary:
    //     Position of the picture among pictures of the same generic type / native code
    //     (default 1 if the picture is one of its kind)
    public int Position { get; set; }


    //
    // Summary:
    //     Native picture code according to TagType convention (numeric : e.g. ID3v2)
    public int NativePicCode { get; set; }

    //
    // Summary:
    //     Native picture code according to TagType convention (string : e.g. APEtag)
    public string? NativePicCodeStr { get; init; }

    //
    // Summary:
    //     Picture description
    public string Description { get; set; } = string.Empty;

    //
    // Summary:
    //     Binary picture data
    public byte[] PictureData { get; set; } = [];

    //
    // Summary:
    //     Hash of binary picture data
    public uint PictureHash { get; set; }

    //
    // Summary:
    //     True if the field has to be deleted in the next IMetaDataIO.Write operation
    public bool MarkedForDeletion { get; set; }

    //
    // Summary:
    //     Freeform transient value to be used by other parts of the library
    public int TransientFlag { get; set; }

    //
    // Summary:
    //     Get the MIME-type associated with the picture
    public string MimeType { get; set; } = string.Empty;


}


public record ComicMetadata()
{
    public bool IsEmpty { get; set; } = true;
    public string TagOrigin { get; set; } = string.Empty;

    public string? Title { get; set; }
    public string? Series { get; set; }
    public int? Issue { get; set; } // Number
    public int? IssueCount { get; set; } // Count
    public int? Volume { get; set; }
    public int? VolumeCount { get; set; }

    public string? AlternateSeries { get; set; }
    public int? AlternateIssue { get; set; }
    public int? AlternateIssueCount { get; set; }

    public string? Summary { get; set; }  // use same way as Summary in CIX 
    public string? Notes { get; set; }

    public int? Month { get; set; }
    public int? Year { get; set; }
    public int? Day { get; set; }

    public string? Publisher { get; set; }
    public string? Imprint { get; set; }
    public string? Genre { get; set; }
    public string[] Tags { get; set; } = [];
    public string? WebLink { get; set; }
    public int? PageCount { get; set; }
    public string? Language { get; set; }  // 2 letters iso code
    public string? Format { get; set; }
    public bool? BlackAndWhite { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Manga Manga { get; set; } = Manga.Unknown;
    public string[] Characters { get; set; } = [];
    public string? Teams { get; set; }
    public string? Locations { get; set; }
    public string? ScanInfo { get; set; }
    public string? StoryArc { get; set; }
    public string? StoryArcNumber { get; set; }
    public string? SeriesGroup { get; set; }
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public AgeRating? MaturityRating { get; set; }
    public string? CommunityRating { get; set; }
    public string? MainCharacterOrTeam { get; set; }
    public string? Review { get; set; }
    public string? GTIN { get; set; }

    public string? Country { get; set; }

    public PageInfo[] Pages { get; set; } = [];

    // Some CoMet-only items
    public double? Price { get; set; }
    public string? IsVersionOf { get; set; }
    public string? Rights { get; set; }
    public string? Identifier { get; set; }
    public int? LastMark { get; set; }
    public string? CoverImage { get; set; }


    public Dictionary<JobTag, string[]> Credits {get;set;}

    public class PageInfo
    {
        public int PageNumber { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PageType PageType { get; set; } = PageType.Unknown;
        public bool DoublePage { get; set; } = false;
        public long ImageSize { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Bookmark { get; set; } = string.Empty;
        public uint? ImageWidth { get; set; }
        public uint? ImageHeight { get; set; }
    }
}

public enum JobTag
{
    Creator,
    Writer,
    Penciller,
    Inker,
    Colorist,
    Letterer,
    Editor,
    CoverArtist,
    Translator
}

public enum PageType
{
    Unknown,
    FrontCover,
    InnerCover,
    Roundup,
    Story,
    Advertisement,
    Editorial,
    Letters,
    Preview,
    BackCover,
    Other,
    Deleted,
}

public enum Manga
{

    Unknown,
    No,
    Yes,
    YesAndRightToLeft,
}

public enum AgeRating
{
    Unknown,
    [EnumMember(Value = "Adults Only 18+")]
    AdultsOnly18,
    [EnumMember(Value = "Early Childhood")]
    EarlyChildhood,
    Everyone,
    [EnumMember(Value = "Everyone 10+")]
    Everyone10,
    G,
    [EnumMember(Value = "Kids to Adults")]
    KidstoAdults,
    M,
    [EnumMember(Value = "MA15+")]
    MA15,
    [EnumMember(Value = "Mature 17+")]
    Mature17,
    PG,
    [EnumMember(Value = "R18+")]
    R18,
    [EnumMember(Value = "Rating Pending")]
    RatingPending,
    Teen,
    [EnumMember(Value = "X18+")]
    X18,
}