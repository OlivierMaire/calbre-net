using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;

public class AdvancedSearchForm
{
    public StringSearchTerm Author = new()
    {
        Key = "author"
    };
    public StringSearchTerm Series = new()
    {
        Key = "series"
    };
    public RatingSearchTerm Rating = new()
    {
        Key = "rating"
    };
    public StringSearchTerm Keyword = new()
    {
        Key = "keyword"
    };
    public StringSearchTerm Tag = new()
    {
        Key = "tag"
    };
    public StringSearchTerm Publisher = new()
    {
        Key = "publisher"
    };
    public ListSearchTerm Language = new()
    {
        Key = "language"
    };
    public ListSearchTerm Format = new()
    {
        Key = "format"
    };
    public List<SearchTerm> CustomColumns { get; set; } = [];

}





public record GetSearchValuesRequest(List<SearchTerm> Terms);
public record GetSearchValuesResponse(List<SearchTerm> Terms);


public static class TermsExt
{
    public static bool HasKey(this List<SearchTerm> terms, string key)
    {
        return terms.Any(t => t.Key == key);
    }

    public static ISearchTerm? Get(this List<SearchTerm> terms, string key)
    {
        return terms.FirstOrDefault(t => t.Key == key);
    }

    public static string ToUrl(this List<SearchTerm> terms)
    {
        string url = string.Empty;
        foreach (var term in terms)
        {
            var operatorValue = string.Empty;

            if (string.IsNullOrEmpty(term.Value))
                continue;

            if (term is NumericSearchTerm numTerm)
                operatorValue = GetEnumMemberValue(numTerm.NumericSearchOperator) + ":";
            if (term is StringSearchTerm stringTerm)
                operatorValue = GetEnumMemberValue(stringTerm.StringSearchOperator) + ":";

            url += $"{term.Key}/{term.Prefix}{operatorValue}{term.Value}/";
        }

        return url;
    }

    public static string? GetEnumMemberValue<T>(this T value)
    where T : Enum
    {
        return typeof(T)
            .GetTypeInfo()
            .DeclaredMembers
            .SingleOrDefault(x => x.Name == value.ToString())
            ?.GetCustomAttribute<EnumMemberAttribute>(false)
            ?.Value;
    }

   

}