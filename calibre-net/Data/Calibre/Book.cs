using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Sort { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public DateTimeOffset? Pubdate { get; set; }

    public double SeriesIndex { get; set; }

    public string? AuthorSort { get; set; }

    public string? Isbn { get; set; }

    public string? Lccn { get; set; }

    public string Path { get; set; } = null!;

    public int Flags { get; set; }

    public string? Uuid { get; set; }

    public bool? HasCover { get; set; }

    public DateTimeOffset LastModified { get; set; }
}
