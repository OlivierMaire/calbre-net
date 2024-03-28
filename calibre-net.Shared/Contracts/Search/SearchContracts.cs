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

    public static List<SearchTerm> ToSearchTerms(this IDictionary<string, string> source)
    {
        List<SearchTerm> terms = [];
        foreach (var (SourceKey, SourceValue) in source)
        {
            SearchTerm term;
            if (SourceValue.StartsWith("s:", StringComparison.InvariantCultureIgnoreCase))
            {
                var value = SourceValue[2..];
                term = new StringSearchTerm();
                var possibleOperator = value.Split(":")[0];
                if (GetValueFromEnumMember<StringOperator>(value.Split(":")[0], out var enumValue))
                {
                    (term as StringSearchTerm).StringSearchOperator = enumValue;
                    value = value[(possibleOperator.Length + 1)..];
                }
                term.Value = value;
            }
            else if (SourceValue.StartsWith("n:", StringComparison.InvariantCultureIgnoreCase))
            {
                var value = SourceValue[2..];
                term = new NumericSearchTerm();
                var possibleOperator = value.Split(":")[0];
                if (GetValueFromEnumMember<NumericOperator>(value.Split(":")[0], out var enumValue))
                {
                    (term as NumericSearchTerm).NumericSearchOperator = enumValue;
                    value = value[(possibleOperator.Length + 1)..];
                }
                term.Value = value;
            }
            else if (SourceValue.StartsWith("r:", StringComparison.InvariantCultureIgnoreCase))
            {
                var value = SourceValue[2..];
                term = new RatingSearchTerm();
                var possibleOperator = value.Split(":")[0];
                if (GetValueFromEnumMember<NumericOperator>(value.Split(":")[0], out var enumValue))
                {
                    (term as RatingSearchTerm).NumericSearchOperator = enumValue;
                    value = value[(possibleOperator.Length + 1)..];
                }
                term.Value = value;
            }
            else if (SourceValue.StartsWith("l:", StringComparison.InvariantCultureIgnoreCase))
            {
                term = new ListSearchTerm();
                term.Value = SourceValue[2..];
            }
            else
            {
                term = new IdSearchTerm();
                term.Value = SourceValue;
            }
            term.Key = SourceKey;
            terms.Add(term);
        }
        return terms;
    }

    private static bool GetValueFromEnumMember<T>(string value, out T enumOutput)
    {
        var type = typeof(T);
        if (type.GetTypeInfo().IsEnum)
        {
            foreach (var name in Enum.GetNames(type))
            {
                var attr = type.GetRuntimeField(name)
                    .GetCustomAttribute<System.Runtime.Serialization.EnumMemberAttribute>(true);
                if (attr != null && attr.Value == value)
                {
                    enumOutput = (T)Enum.Parse(type, name);
                    return true;
                }
            }
            enumOutput = default!;
            return false;
        }

        throw new InvalidOperationException("Not Enum");
    }

}