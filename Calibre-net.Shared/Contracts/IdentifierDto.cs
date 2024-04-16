using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;
public partial class IdentifierDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("book")]
    public int Book { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("val")]
    public string Val { get; set; } = string.Empty;

    [JsonPropertyName("bookObject")]
    public BookDto? BookObject { get; set; } = null;

}

public partial class IdentifierDto
{
    public string TypeName =>
     Type.ToLower() switch
     {
         "amazon" => "Amazon",
         "isbn" => "ISBN",
         "doi" => "DOI",
         "douban" => "Douban",
         "goodreads" => "Goodreads",
         "babelio" => "Babelio",
         "google" => "Google Books",
         "kobo" => "Kobo",
         "litres" => "ЛитРес",
         "issn" => "ISSN",
         "isfdb" => "ISFDB",
         "lubimyczytac" => "Lubimyczytac",
         "databazeknih" => "Databáze knih",
         _ => Type.ToLower().StartsWith("amazon_") ? $"Amazon.{Type.Substring(7)}"  : Type
     };

     public string Link =>
     Type.ToLower() switch
     {
         "amazon" or "asin" => $"https://amazon.com/dp/{Val}",
         "isbn" => $"https://www.worldcat.org/isbn/{Val}",
         "doi" => $"https://dx.doi.org/{Val}",
         "douban" => $"https://book.douban.com/subject/{Val}",
         "goodreads" => $"https://www.goodreads.com/book/show/{Val}",
         "babelio" => $"https://www.babelio.com/livres/titre/{Val}",
         "google" => $"https://books.google.com/books?id={Val}",
         "kobo" => $"https://www.kobo.com/ebook/{Val}",
         "litres" => $"https://www.litres.ru/{Val}",
         "issn" => $"https://portal.issn.org/resource/ISSN/{Val}",
         "isfdb" => $"http://www.isfdb.org/cgi-bin/pl.cgi?{Val}",
         "lubimyczytac" => $"https://lubimyczytac.pl/ksiazka/{Val}/ksiazka",
         "databazeknih" => $"https://www.databazeknih.cz/knihy/{Val}",
         _ => new Func<string>(() =>  {
            var type =  Type.ToLower();
            var val =  Val.ToLower();
            if (type.StartsWith("amazon_"))
                return $"https://amazon.{Type.Substring(7)}/dp/{Val}";
            if (val.StartsWith("javascript:"))
                return System.Net.WebUtility.UrlEncode(Val);
            if (val.StartsWith("data:"))
                return val.Split(",")[0];
             return string.Empty;
         })()
     };
}