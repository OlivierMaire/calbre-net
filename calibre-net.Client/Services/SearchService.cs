using calibre_net.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace calibre_net.Client.Services;

[ScopedRegistration]
public class SearchService(NavigationManager navigationManager)
{
    private readonly NavigationManager _navigationManager = navigationManager;

    public string GetSearchUrl(ISearchable searchable)
    {
        var fullUri = _navigationManager.Uri;
        if (fullUri.Contains(searchable.SearchBase))
        {
            var pageRoute = fullUri.Split(searchable.SearchBase)[1];
            var dict = pageRoute.ParseStringToDictionary();
            var kv = searchable.SearchUrl.Split("/");
            if (kv.Length == 2)
                dict[kv[0]] = kv[1];

            return searchable.SearchBase + string.Join("/", dict.Select(d => $"{d.Key}/{d.Value}"));
        }
        else
            return searchable.SearchBase + searchable.SearchUrl;

    }

  
}