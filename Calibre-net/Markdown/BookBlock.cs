
using Markdig.Helpers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Calibre_net.Markdown.Extensions;

/// <summary>
/// A block representing an alert quote block.
/// </summary>
public class BookBlock : ContainerInline
{
    /// <summary>
    /// Creates a new instance of this block.
    /// </summary>
    /// <param name="kind"></param>
    public BookBlock(StringSlice bookId) : base()
    {
        BookId = bookId;
    }

    /// <summary>
    /// Gets or sets the kind of the alert block (e.g `NOTE`, `TIP`, `IMPORTANT`, `WARNING`, `CAUTION`)
    /// </summary>
    public StringSlice BookId { get; set; }

    
}