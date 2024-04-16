
namespace Calibre_net.Models;

public class PasskeyModel
{
    public int Id {get;set;}
    public string ProviderName { get; internal set; } = string.Empty;
    public string Name { get; internal set; } = string.Empty;
    public DateTimeOffset CreatedDate { get; internal set; }
    public DateTimeOffset? LastUsedDate { get; internal set; }
    public string? LastUsedDevice { get; internal set; }
    public string? LastUsedLocation { get; internal set; }
    public string? ProviderIconLight { get; internal set; }
    public string? ProviderIconDark { get; internal set; }
    public Guid AaGuid { get; internal set; }

    public string Title => string.IsNullOrEmpty(Name) ? ProviderName : Name;
    public string SubTitle => string.IsNullOrEmpty(Name) ? string.Empty : $"({ProviderName})";

}