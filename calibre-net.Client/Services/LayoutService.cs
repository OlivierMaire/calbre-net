using System.Globalization;
using calibre_net.Client.ApiClients;
using calibre_net.Client.Enums;
using Microsoft.JSInterop;
using MudBlazor;

namespace calibre_net.Client.Services;

[ScopedRegistration]
public class LayoutService : IAsyncDisposable
{
    private readonly IUserPreferencesService _userPreferencesService;
    private readonly CultureClient _cultureClient;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;
    private UserPreferences? _userPreferences = null;
    private bool _systemPreferences;

    public bool IsRTL { get; private set; }
    public DarkLightMode DarkModeToggle = DarkLightMode.System;

    public bool IsDarkMode { get; private set; }

    public MudTheme? CurrentTheme { get; private set; }

    public LayoutService(IUserPreferencesService userPreferencesService, IJSRuntime jsRuntime, CultureClient cultureClient)
    {
        _userPreferencesService = userPreferencesService;
        _cultureClient = cultureClient;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                 "import", "./Components/Layout/Parts/ThemeToggleButton.razor.js").AsTask());
    }

    public void SetDarkMode(bool value)
    {
        IsDarkMode = value;
    }

    public async Task ApplyUserPreferences(bool isDarkModeDefaultTheme)
    {
        Console.WriteLine($"ApplyUserPreferences {isDarkModeDefaultTheme}");
        _systemPreferences = isDarkModeDefaultTheme;
        _userPreferences ??= await _userPreferencesService.LoadUserPreferences();
        Console.WriteLine($"LoadUserPreferences {_userPreferences.DarkLightTheme}");
        Console.WriteLine($"DarkModeToggle {DarkModeToggle}");

        if (_userPreferences != null)
        {
            DarkModeToggle = _userPreferences.DarkLightTheme;
            IsDarkMode = _userPreferences.DarkLightTheme switch
            {
                DarkLightMode.Dark => true,
                DarkLightMode.Light => false,
                DarkLightMode.System => isDarkModeDefaultTheme,
                _ => IsDarkMode
            };
            IsRTL = _userPreferences.RightToLeft;
        }
        else
        {
            IsDarkMode = isDarkModeDefaultTheme;
            _userPreferences = new UserPreferences { DarkLightTheme = DarkLightMode.System };
            await _userPreferencesService.SaveUserPreferences(_userPreferences);
        }
        Console.WriteLine($"DarkModeToggle {DarkModeToggle}");
        OnMajorUpdateOccured();
    }

    public Task OnSystemPreferenceChanged(bool newValue)
    {
        _systemPreferences = newValue;
        if (DarkModeToggle == DarkLightMode.System)
        {
            IsDarkMode = newValue;
            OnMajorUpdateOccured();
        }
        return Task.CompletedTask;
    }

    public event EventHandler? MajorUpdateOccured;

    private void OnMajorUpdateOccured() => MajorUpdateOccured?.Invoke(this, EventArgs.Empty);

    public async Task ToggleDarkMode()
    {
        _userPreferences ??= await _userPreferencesService.LoadUserPreferences();
        switch (DarkModeToggle)
        {
            case DarkLightMode.System:
                DarkModeToggle = DarkLightMode.Light;
                IsDarkMode = false;
                break;
            case DarkLightMode.Light:
                DarkModeToggle = DarkLightMode.Dark;
                IsDarkMode = true;
                break;
            case DarkLightMode.Dark:
                DarkModeToggle = DarkLightMode.System;
                IsDarkMode = _systemPreferences;
                break;
        }

        _userPreferences.DarkLightTheme = DarkModeToggle;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
        await SaveCookie();
        OnMajorUpdateOccured();
    }

    public async Task ToggleRightToLeft()
    {
        _userPreferences ??= await _userPreferencesService.LoadUserPreferences();
        IsRTL = !IsRTL;
        _userPreferences.RightToLeft = IsRTL;
        await _userPreferencesService.SaveUserPreferences(_userPreferences);
        OnMajorUpdateOccured();
    }

    public void SetBaseTheme(MudTheme theme)
    {
        CurrentTheme = theme;
        OnMajorUpdateOccured();
    }


    public async Task SaveCookie()
    {
        var module = await moduleTask.Value;
        await module.InvokeAsync<string>("blazorDarkTheme.set", IsDarkMode);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}