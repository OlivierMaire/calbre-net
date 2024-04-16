using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;


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
    public const string BOOK_TAG = "book";
    public const string AUTHOR_TAG = "author";
    public const string AUTHOR_ICON = "icon-cn-feather_1";
    public const string SERIES_TAG = "series";
    public const string SERIES_ICON = MudBlazor.Icons.Material.Outlined.LibraryBooks;
    public const string TAG_TAG = "tag";
    public const string TAG_ICON = MudBlazor.Icons.Material.Outlined.Tag;
    public const string PUBLISHER_TAG = "publisher";
    public const string PUBLISHER_ICON = "icon-cn-print-press";
    public const string LANGUAGE_TAG = "language";
    public const string LANGUAGE_ICON = MudBlazor.Icons.Material.Outlined.Flag;
    public const string RATING_TAG = "rating";
    public const string RATING_ICON = MudBlazor.Icons.Material.Outlined.StarOutline;
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

public static class SearchOrdersConstants
{
    public const string BOOK_LASTMODIFIED_TAG = "bl";
    public const string BOOK_TITLE_TAG = "bt";
    public const string AUTHOR_SORT_TAG = "as";
    public const string BOOK_PUBDATE_TAG = "bp";
    public const string SERIES_SORT_TAG = "ss";
    public const string RATING_TAG = "rt";
    public const string PUBLISHER_SORT_TAG = "ps";
    public const string LANGUAGE_TAG = "lg";
    public const string FORMAT_TAG = "ft";
    public const string CUSTOM_COLUMN_TAG = "cc_";

    public static Dictionary<string, SearchOrderModel> DefaultSearchOrders =
    new()
    {
        {
            BOOK_LASTMODIFIED_TAG,
            new (){
            Type = BOOK_LASTMODIFIED_TAG,
            Key = SearchTermsConstants.BOOK_TAG,
            PropertyName = nameof(BookDto.LastModified),
            Icon = MudBlazor.Icons.Material.Outlined.DateRange
            }
        },
        {
            BOOK_TITLE_TAG,
            new (){
            Type = BOOK_TITLE_TAG,
            Key = SearchTermsConstants.BOOK_TAG,
            PropertyName = nameof(BookDto.Title),
            Icon = MudBlazor.Icons.Material.Outlined.ShortText
            }
        },
        {
            AUTHOR_SORT_TAG,
            new (){
            Type = AUTHOR_SORT_TAG,
            Key = SearchTermsConstants.AUTHOR_TAG,
            PropertyName = nameof(BookDto.AuthorSort),
            Icon = SearchTermsConstants.AUTHOR_ICON
            }
        },
        {
            BOOK_PUBDATE_TAG,
            new (){
            Type = BOOK_PUBDATE_TAG,
            Key = SearchTermsConstants.BOOK_TAG,
            PropertyName = nameof(BookDto.Pubdate),
            Icon = MudBlazor.Icons.Material.Outlined.DateRange
            }
        },
        {
            SERIES_SORT_TAG,
            new (){
            Type = SERIES_SORT_TAG,
            Key = SearchTermsConstants.SERIES_TAG,
            PropertyName = nameof(SeriesDto.Sort),
            Icon =  SearchTermsConstants.SERIES_ICON
            }
        },
        {
            RATING_TAG,
            new (){
            Type = RATING_TAG,
            Key = SearchTermsConstants.RATING_TAG,
            PropertyName = nameof(RatingDto.Rating),
            Icon =  SearchTermsConstants.RATING_ICON
            }
        },
        {
            PUBLISHER_SORT_TAG,
            new (){
            Type = PUBLISHER_SORT_TAG,
            Key = SearchTermsConstants.PUBLISHER_TAG,
            PropertyName = nameof(PublisherDto.Sort),
            Icon =  SearchTermsConstants.PUBLISHER_ICON
            }
        },
        {
            LANGUAGE_TAG,
            new (){
            Type = LANGUAGE_TAG,
            Key = SearchTermsConstants.LANGUAGE_TAG,
            PropertyName = nameof(LanguageDto.LangCode),
            Icon =  SearchTermsConstants.LANGUAGE_ICON
            }
        },
        {
            FORMAT_TAG,
            new (){
            Type = FORMAT_TAG,
            Key = SearchTermsConstants.FORMAT_TAG,
            PropertyName = nameof(DataDto.Format),
            Icon =  SearchTermsConstants.FORMAT_ICON
            }
        },
        {
            CUSTOM_COLUMN_TAG,
            new (){
            Type = CUSTOM_COLUMN_TAG,
            Key = "cc_",
            PropertyName = string.Empty,
            Icon =  MudBlazor.Icons.Material.Outlined.FilterNone
            }
        }
    };

}



public class SearchOrder
{
    public string Key { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public bool Ascending { get; set; } = true;

    // public bool ServerSide { get; set; } = false;
}

public class SearchOrderModel : SearchOrder, ICloneable
{
    public string Type { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int Position { get; set; } = 0;

    public object Clone()
    {
        return new SearchOrderModel{
            Ascending = this.Ascending,
            Icon = this.Icon,
            Key = this.Key,
            Position = this.Position,
            PropertyName = this.PropertyName,
            Type = this.Type
        };
    }
}