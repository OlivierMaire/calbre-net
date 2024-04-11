using calibre_net.Client.ApiClients;
using calibre_net.Shared.Contracts;
using Microsoft.AspNetCore.Components;

namespace calibre_net.Client.Services;

[ScopedRegistration]
public class SearchService(NavigationManager navigationManager, CategoryClient categoryClient)
{
    private readonly NavigationManager _navigationManager = navigationManager;
    private readonly CategoryClient _categoryClient = categoryClient;

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

    public async Task<List<KeyValuePair<string, string>>> GetLanguages()
    {
        var languages = await _categoryClient.LanguagesAsync();
        if (languages != null)
        {
            return languages.Languages.Select(l => KeyValuePair.Create(l.Id.ToString(), l.LangCode)).ToList();
        }
        return [];
    }

    public async Task<List<KeyValuePair<string, string>>> GetFormats()
    {
        var formats = await _categoryClient.FormatsAsync();
        if (formats != null)
        {
            return formats.Formats.Select(l => KeyValuePair.Create(l.Format, l.Format)).ToList();
        }
        return [];
    }

}