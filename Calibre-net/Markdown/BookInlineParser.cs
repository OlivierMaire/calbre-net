using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Calibre_net.Markdown.Extensions;

/// <summary>
/// An inline parser for a book inline (e.g. `{B}(0)`).
/// </summary>
/// <seealso cref="InlineParser" />
public class BookInlineParser : InlineParser
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookInlineParser"/> class.
    /// </summary>
    public BookInlineParser()
    {
        OpeningCharacters = ['@'];
    }

    public override bool Match(InlineProcessor processor, ref StringSlice slice)
    {
        // We expect the alert to be the first child of a quote block. Example:
        // > {B}(0)
        if (processor.Block is not ParagraphBlock paragraphBlock ||
            // paragraphBlock.Parent is not QuoteBlock quoteBlock ||
            paragraphBlock.Inline?.FirstChild != null)
        {
            return false;
        }

        // var saved = slice;
        var c = slice.NextChar();
        if (c != 'B')
        {
            return false;
        }
 
        c = slice.NextChar();
        if (c != '(')
        {
            // slice = saved;
            return false;
        }


        c = slice.NextChar();
        var start = slice.Start;
        var end = start;
        while (c.IsDigit())
        {
            end = slice.Start;
            c = slice.NextChar();
        }

        // We need at least one character
        if (c != ')' || start == end)
        {
            // slice = saved;
            return false;
        }

        var bookId = new StringSlice(slice.Text, start, end);
        c = slice.NextChar(); // Skip ]

        // start = slice.Start;
        // while (true)
        // {
        //     if (c == '\0' || c == '\n' || c == '\r')
        //     {
        //         end = slice.Start;
        //         if (c == '\r')
        //         {
        //             c = slice.NextChar(); // Skip \r
        //             if (c == '\0' || c == '\n')
        //             {
        //                 end = slice.Start;
        //                 if (c == '\n')
        //                 {
        //                     slice.NextChar(); // Skip \n
        //                 }
        //             }
        //         }
        //         else if (c == '\n')
        //         {
        //             slice.NextChar(); // Skip \n
        //         }
        //         break;
        //     }
        //     else if (!c.IsSpaceOrTab())
        //     {
        //         slice = saved;
        //         return false;
        //     }

        //     c = slice.NextChar();
        // }

        var bookBlock = new BookBlock(bookId)
        {
            // Span = quoteBlock.Span,
            // TriviaSpaceAfterKind = new StringSlice("hello"),
            // Line = quoteBlock.Line,
            // Column = quoteBlock.Column,
        };

        bookBlock.GetAttributes().AddClass("markdown-book");
        bookBlock.GetAttributes().AddClass($"markdown-book-{bookId.ToString().ToLowerInvariant()}");

        // Replace the quote block with the alert block
        // var parentQuoteBlock = quoteBlock.Parent!;
        // var indexOfQuoteBlock = parentQuoteBlock.IndexOf(quoteBlock);
        // parentQuoteBlock[indexOfQuoteBlock] = bookBlock;

        // while (quoteBlock.Count > 0)
        // {
        //     var block = quoteBlock[0];
        //     quoteBlock.RemoveAt(0);
        //     bookBlock.Add(block);
        // }

        // Workaround to replace the parent container
        // Experimental API, so we are keeping it internal for now until we are sure it's the way we want to go
        // processor.ReplaceParentContainer(quoteBlock, bookBlock);

        processor.Inline = bookBlock;


        return true;
    }
}