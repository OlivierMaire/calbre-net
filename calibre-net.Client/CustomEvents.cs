
using Microsoft.AspNetCore.Components;

namespace calibre_net.CustomEvents;

[EventHandler("onthemechangedevent", typeof(ThemeChangedEvent), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{

}
  
  public class CustomEventArgs : EventArgs
{
    public string? CustomProperty1 {get; set;}
    // public string? CustomProperty2 {get; set;} 
}

public class ThemeChangedEvent: EventArgs{
    public bool IsDarkMode {get;set;}
}