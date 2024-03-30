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

  

  
}