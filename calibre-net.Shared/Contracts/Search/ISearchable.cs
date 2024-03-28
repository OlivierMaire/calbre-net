namespace calibre_net.Shared.Contracts;

public interface ISearchable
{
    public string SearchUrl { get; }
    public string SearchBase { get; }
}

public abstract class Searchable : ISearchable
{
    public virtual string SearchUrl => throw new NotImplementedException();

    public string SearchBase => "/books/";
}

public static class SearchableExtensions
{
   
    public static Dictionary<string, string> ParseStringToDictionary(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return [];
        var dictionary = new Dictionary<string, string>();
        var pairs = input.Split('/');

        for (int i = 0; i + 1 < pairs.Length; i += 2)
        {
            string key = pairs[i];
            string value = pairs[i + 1];

            // You can convert the value to the appropriate type (e.g., int, double, etc.) if needed.
            dictionary[key] =System.Net.WebUtility.UrlDecode(value); 
        }

        return dictionary;
    }
}