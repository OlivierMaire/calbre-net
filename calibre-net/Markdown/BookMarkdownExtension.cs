
using Calibre_net.Services;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Calibre_net.Markdown.Extensions;

/// <summary>
/// Extension for adding books to a Markdown pipeline.
/// </summary>
public class BookExtension(BookService bookService) : IMarkdownExtension
{
    private readonly BookService _bookService = bookService;

    /// <summary>
    /// Gets or sets the delegate to render the book.
    /// </summary>
    public Action<HtmlRenderer, StringSlice>? RenderBook { get; set; }

    /// <inheritdoc />
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        var inlineParser = pipeline.InlineParsers.Find<BookInlineParser>();
        if (inlineParser == null)
        {
            pipeline.InlineParsers.InsertBefore<LinkInlineParser>(new BookInlineParser());
        }
    }

    /// <inheritdoc />
    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        var blockRenderer = renderer.ObjectRenderers.FindExact<BookBlockRenderer>();
        if (blockRenderer == null)
        {
            renderer.ObjectRenderers.InsertBefore<QuoteBlockRenderer>(new BookBlockRenderer(_bookService));
        }
    }
}

public static class BookMarkdownSetup{
     
    //
    // Summary:
    //     Uses this extension to enable book blocks.
    //
    // Parameters:
    //   pipeline:
    //     The pipeline.
    //
    //   renderKind:
    //     Replace the default renderer for the kind with a custom renderer
    //
    // Returns:
    //     The modified pipeline
    public static MarkdownPipelineBuilder UseBookBlocks(this MarkdownPipelineBuilder pipeline, 

    Action<HtmlRenderer, StringSlice>? renderBook = null)
    {
        // pipeline.Use<BookExtension>();//.Extensions.ReplaceOrAdd<BookExtension>(new BookExtension());
        return pipeline;
    }
}