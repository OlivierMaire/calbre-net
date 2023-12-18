using Microsoft.Extensions.Localization;

namespace calibre_net.Shared.Resources;

public class JsonStringLocalizerOptions : LocalizationOptions{

    
public bool ShowKeyNameIfEmpty { get; set;}
public bool ShowDefaultIfEmpty { get; set;}

}