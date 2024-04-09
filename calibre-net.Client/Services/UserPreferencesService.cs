using calibre_net.Client.Enums;
using Blazored.LocalStorage;
using System.Globalization;

namespace calibre_net.Client.Services;

public interface IUserPreferencesService
{
    /// <summary>
    /// Saves UserPreferences in local storage
    /// </summary>
    /// <param name="userPreferences">The userPreferences to save in the local storage</param>
    public Task SaveUserPreferences(UserPreferences userPreferences);

    /// <summary>
    /// Loads UserPreferences in local storage
    /// </summary>
    /// <returns>UserPreferences object. Null when no settings were found.</returns>
    public Task<UserPreferences> LoadUserPreferences();
}

[ScopedRegistration]
public class UserPreferencesService : IUserPreferencesService
{
    private readonly ILocalStorageService _localStorage;
    private const string Key = "userPreferences";

    public UserPreferencesService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task SaveUserPreferences(UserPreferences userPreferences)
    {
        await _localStorage.SetItemAsync(Key, userPreferences);
    }

    public async Task<UserPreferences> LoadUserPreferences()
    {
        return await _localStorage.GetItemAsync<UserPreferences>(Key) ?? new();
    }
}

public class UserPreferences
{
    /// <summary>
    /// Set the direction layout of the docs to RTL or LTR. If true RTL is used
    /// </summary>
    public bool RightToLeft { get; set; }

    /// <summary>
    /// The current dark light mode that is used
    /// </summary>
    public DarkLightMode DarkLightTheme { get; set; }

}
