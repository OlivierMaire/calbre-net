using System.ComponentModel;
using System.Reflection;
using calibre_net.Shared.Contracts;

namespace calibre_net.Shared;
public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, string> source)
        where T : class, new()
    {
        var resultObject = new T();
        var ObjectType = resultObject.GetType();

        foreach (var (SourceKey, SourceValue) in source)
        {
            var property = ObjectType
                      .GetProperty(SourceKey, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                continue;
            Type propertyType = property.PropertyType;

            if (propertyType == typeof(string))
            {
                property.SetValue(resultObject, SourceValue.ToString(), null);
                continue;
            }

            //Nullable properties have to be treated differently, since we 
            //  use their underlying property to set the value in the object
            if (propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                //if it's null, just set the value from the reserved word null, and return
                if (SourceValue == null)
                {
                    property.SetValue(resultObject, null, null);
                    continue;
                }

                //Get the underlying type property instead of the nullable generic
                propertyType = new NullableConverter(property.PropertyType).UnderlyingType;
                property.SetValue(resultObject, Convert.ChangeType(SourceValue, propertyType), null);
            }
        }

        return resultObject;
    }

    public static IDictionary<string, object?> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        return source.GetType().GetProperties(bindingAttr)
        .ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );

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