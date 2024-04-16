namespace Calibre_net.Shared.Resources;

public class SupportedCulturesOptions
{
    public CultureOption[] SupportedCultureOptions { get; set; } = [new CultureOption {Value = "en-US", DisplayName = "English" },
        new CultureOption {Value = "fr-FR",  DisplayName = "Français"}, new CultureOption{ Value =  "ja-JP", DisplayName = "日本語" }];
    public string[] SupportedCultures => SupportedCultureOptions.Select(c => c.Value).ToArray();
}

public class CultureOption
{
    public string Value { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}
