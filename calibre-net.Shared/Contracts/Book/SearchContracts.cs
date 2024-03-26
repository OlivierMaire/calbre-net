using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;

public record SearchRequest
{

    public int? Author { get; set; }
    public int? Series { get; set; }
    public int? Rating { get; set; }
    public int? RatingValue { get; set; }
    public NumericOperator RatingValueOperator { get; set; } = NumericOperator.Equals;
    public string? Keyword { get; set; }
    public int? Tag { get; set; }
    public int? Publisher { get; set; }
    public int? Language { get; set; }
    public string? Format { get; set; }
    public Dictionary<int, int?> CustomColumn { get; set; } = [];

    public List<SearchTerm> Terms { get; set; } = [];
}

public enum NumericOperator
{
    [EnumMember(Value = "=")]
    Equals,
    [EnumMember(Value = ">")]
    GreaterThan,
    [EnumMember(Value = "<")]
    LesserThan,
    [EnumMember(Value = ">=")]
    GreaterOrEquals,
    [EnumMember(Value = "<=")]
    LesserOrEquals,
}

public enum StringOperator
{
    Equals,
    Contains,
    StartsWith
}


public class SearchTerm
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = null!;

    public bool IsNumeric { get; set; }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public NumericOperator? NumericSearchOperator { get; set; }
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public StringOperator? StringSearchOperator { get; set; }

    public string KeyName { get; set; } = string.Empty;
    public string ValueName { get; set; } = string.Empty;
}

public record GetSearchValuesRequest(List<SearchTerm> Terms);
public record GetSearchValuesResponse(List<SearchTerm> Terms);


public static class TermsExt
{
    public static bool HasKey(this List<SearchTerm> terms, string key)
    {
        return terms.Any(t => t.Key == key);
    }

    public static SearchTerm? Get(this List<SearchTerm> terms, string key)
    {
        return terms.FirstOrDefault(t => t.Key == key);
    }

    

}