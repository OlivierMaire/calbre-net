using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;


public interface ISearchTerm
{
    public string Key { get; set; }
    public string Value { get; set; }

    public string Prefix { get; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(IdSearchTerm), "IdSearchTerm")]
[JsonDerivedType(typeof(StringSearchTerm), "StringSearchTerm")]
[JsonDerivedType(typeof(NumericSearchTerm), "NumericSearchTerm")]
[JsonDerivedType(typeof(RatingSearchTerm), "RatingSearchTerm")]
[JsonDerivedType(typeof(ListSearchTerm), "ListSearchTerm")]
[JsonDerivedType(typeof(CustomColumnSearchTerm), "CustomColumnSearchTerm")]
public abstract class SearchTerm : ISearchTerm
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;

    public virtual string Prefix => "";
}
public class IdSearchTerm : SearchTerm
{
    public string ValueDisplayName { get; set; } = string.Empty;
} // default Id Search

public class StringSearchTerm : SearchTerm // string search
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public StringOperator StringSearchOperator { get; set; } = StringOperator.Contains;
    public override string Prefix => "s:";
}

public class NumericSearchTerm : SearchTerm // numeric search
{
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public NumericOperator NumericSearchOperator { get; set; } = NumericOperator.Equals;
    public override string Prefix => "n:";

    public int IntValue
    {
        get =>
        (int)(decimal.Parse(string.IsNullOrEmpty(this.Value) ? "0" : this.Value, System.Globalization.CultureInfo.InvariantCulture) * 2);
        set
        {
            this.Value = (value / 2.0m).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

public class RatingSearchTerm : NumericSearchTerm
{

    public override string Prefix => "r:";

} // rating (numeric search)

public class ListSearchTerm : SearchTerm
{
    public override string Prefix => "l:";
    public List<KeyValuePair<string, string>> Values { get; set; } = [];
}

public class CustomColumnSearchTerm : StringSearchTerm
{
    public int ColumnId { get; set; } = 0;

    public string KeyName { get; set; } = string.Empty;
}

public enum StringOperator
{
    [EnumOperator(Value = "=,VALUE")]
    [EnumMember(Value = "=")]
    Equals,
    [EnumOperator(Value = "like,%VALUE%")]
    [EnumMember(Value = "c")]
    Contains,
    [EnumOperator(Value = "like,VALUE%")]
    [EnumMember(Value = "sw")]
    StartsWith,
    [EnumOperator(Value = "like,%VALUE")]
    [EnumMember(Value = "ew")]
    EndsWith,
    [EnumOperator(Value = "<>,VALUE")]
    [EnumMember(Value = "!=")]
    NotEquals,
    [EnumOperator(Value = "not like,%VALUE%")]
    [EnumMember(Value = "!c")]
    NotContains,
    [EnumOperator(Value = "not like,VALUE%")]
    [EnumMember(Value = "!sw")]
    NotStartsWith,
    [EnumOperator(Value = "not like,%VALUE")]
    [EnumMember(Value = "!ew")]
    NotEndsWith,
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


[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class EnumOperatorAttribute : Attribute
{
    public string? Value { get; set; }
}


public static class SearchTermsConstants
{
    public const string AUTHOR_TAG = "author";
    public const string AUTHOR_ICON = "icon-cn-feather_1";
    public const string SERIES_TAG = "series";
    public const string SERIES_ICON = MudBlazor.Icons.Material.Filled.LibraryBooks;
    public const string TAG_TAG = "tag";
    public const string TAG_ICON = MudBlazor.Icons.Material.Filled.Tag;
    public const string PUBLISHER_TAG = "publisher";
    public const string PUBLISHER_ICON = "icon-cn-print-press";
    public const string LANGUAGE_TAG = "language";
    public const string LANGUAGE_ICON = MudBlazor.Icons.Material.Filled.Flag;
    public const string RATING_TAG = "rating";
    public const string RATING_ICON = MudBlazor.Icons.Material.Filled.StarOutline;
    public const string FORMAT_TAG = "format";
    public const string FORMAT_ICON = MudBlazor.Icons.Custom.FileFormats.FileDocument;

    public const string KEYWORD_TAG = "keyword";

    public static string GetIcon(this ISearchTerm term)
    {
        var icon = term.Key switch
        {
            AUTHOR_TAG => AUTHOR_ICON,
            SERIES_TAG => SERIES_ICON,
            TAG_TAG => TAG_ICON,
            PUBLISHER_TAG => PUBLISHER_ICON,
            LANGUAGE_TAG => LANGUAGE_ICON,
            RATING_TAG => RATING_ICON,
            FORMAT_TAG => FORMAT_ICON,
            _ => string.Empty
        };
        return icon;
    }

}
