using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Calibre_net.Shared.Contracts;

public class AdvancedSearchForm
{
    public StringSearchTerm Author = new()
    {
        Key = SearchTermsConstants.AUTHOR_TAG
    };
    public StringSearchTerm Series = new()
    {
        Key = SearchTermsConstants.SERIES_TAG
    };
    public RatingSearchTerm Rating = new()
    {
        Key = SearchTermsConstants.RATING_TAG
    };
    public StringSearchTerm Keyword = new()
    {
        Key = SearchTermsConstants.KEYWORD_TAG
    };
    public StringSearchTerm Tag = new()
    {
        Key = SearchTermsConstants.TAG_TAG
    };
    public StringSearchTerm Publisher = new()
    {
        Key = SearchTermsConstants.PUBLISHER_TAG
    };
    public ListSearchTerm Language = new()
    {
        Key = SearchTermsConstants.LANGUAGE_TAG
    };
    public ListSearchTerm Format = new()
    {
        Key = SearchTermsConstants.FORMAT_TAG
    };
    public List<SearchTerm> CustomColumns { get; set; } = [];

    public void Clear()
    {
        this.Author.Value = string.Empty;
        this.Series.Value = string.Empty;
        this.Rating.Value = string.Empty;
        this.Rating.IntValue = 0;
        this.Keyword.Value = string.Empty;
        this.Tag.Value = string.Empty;
        this.Publisher.Value = string.Empty;
        this.Language.Value = string.Empty;
        this.Format.Value = string.Empty;
        foreach (var cc in this.CustomColumns)
        {
            cc.Value = string.Empty;
        }
    }

}





public record GetSearchValuesRequest(List<SearchTerm> Terms, List<SearchOrder> Orders);
public record GetSearchValuesResponse(List<SearchTerm> Terms);


public static class TermsExt
{
    public static bool HasKey(this List<SearchTerm> terms, string key)
    {
        return terms.Any(t => t.Key == key);
    }

    public static bool HasKey(this List<SearchOrder> orders, string key)
    {
        return orders.Any(t => t.Key == key);
    }

    public static ISearchTerm? Get(this List<SearchTerm> terms, string key)
    {
        return terms.FirstOrDefault(t => t.Key == key);
    }

    public static string ToUrl(this List<SearchTerm> terms, string url = "")
    {
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
                    if (term is StringSearchTerm t) t.StringSearchOperator = enumValue;
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
                    if (term is NumericSearchTerm t) t.NumericSearchOperator = enumValue;
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
                    if (term is RatingSearchTerm t) t.NumericSearchOperator = enumValue;
                    value = value[(possibleOperator.Length + 1)..];
                }
                term.Value = value;
            }
            else if (SourceValue.StartsWith("l:", StringComparison.InvariantCultureIgnoreCase))
            {
                term = new ListSearchTerm
                {
                    Value = SourceValue[2..]
                };
            }
            else
            {
                term = new IdSearchTerm
                {
                    Value = SourceValue
                };
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
                var attr = type.GetRuntimeField(name)?
                    .GetCustomAttribute<EnumMemberAttribute>(true);
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


    public static List<SearchOrderModel> ToSearchOrderModel(this string orderString)
    {
        var orders = orderString.Split(';');
        return orders.Select(o =>
        {
            if (string.IsNullOrEmpty(o))
                return null;
            var param = o.Split(':');
            SearchOrdersConstants.DefaultSearchOrders.TryGetValue(param[2], out var searchOrder);
            if (searchOrder == null && param[2].StartsWith(SearchOrdersConstants.CUSTOM_COLUMN_TAG))
            {
                searchOrder = SearchOrdersConstants.DefaultSearchOrders[SearchOrdersConstants.CUSTOM_COLUMN_TAG];
            }
            if (searchOrder == null)
                return null;
            searchOrder = (SearchOrderModel)searchOrder.Clone();
            searchOrder.Ascending = param[1] == "a";
            int.TryParse(param[0], out var position);
            searchOrder.Position = position;
            if (searchOrder.Key == SearchOrdersConstants.CUSTOM_COLUMN_TAG)
            {
                searchOrder.Key = param[2];
                searchOrder.Type = SearchOrdersConstants.CUSTOM_COLUMN_TAG + param[2].ToString();
                searchOrder.PropertyName = "Name";
            }
            return searchOrder;
        }).Where(so => so != null).ToList() as List<SearchOrderModel>;
    }

    public static List<SearchOrderModel> ApplyFromQuery(this List<SearchOrderModel> orders, string orderString)
    {
        var ordersString = orderString.Split(';');
        foreach (var o in ordersString)
        {
            if (string.IsNullOrEmpty(o))
                continue;
            var param = o.Split(':');
            SearchOrderModel? searchOrder = orders.FirstOrDefault(os => os.Type == param[2]);
            if (searchOrder != null)
            {
                searchOrder.Ascending = param[1] == "a";
                int.TryParse(param[0], out var position);
                searchOrder.Position = position;
            }

        }

        return orders;
    }

    public static List<SearchOrderModel> ApplyDefaults(this List<SearchOrderModel> orders)
    {
        var searchOrder = orders.FirstOrDefault(os => os.Type == SearchOrdersConstants.BOOK_PUBDATE_TAG);
        if (searchOrder != null)
        {
            searchOrder.Ascending = false;
            searchOrder.Position = 1;
        }

        searchOrder = orders.FirstOrDefault(os => os.Type == SearchOrdersConstants.SERIES_SORT_TAG);
        if (searchOrder != null)
        {
            searchOrder.Ascending = true;
            searchOrder.Position = 2;
        }
        return orders;
    }

    public static string ToUrl(this List<SearchOrderModel> orders, string url = "")
    {

        var orderString = orders.Where(o => o.Position > 0).OrderBy(o => o.Position)
            .Select(o => $"{o.Position}:{(o.Ascending ? 'a' : 'd')}:{o.Type}");

        return url.TrimEnd('/') + "/order/" + string.Join(';', orderString);
    }

}