
using System.Text.Json.Serialization;
using Calibre_net.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Calibre_net.Client.CustomEvents;

[EventHandler("onthemechangedevent", typeof(ThemeChangedEvent), enableStopPropagation: true, enablePreventDefault: true)]
[EventHandler("oncalibremessageevent", typeof(CalibreMessageEvent), enableStopPropagation: true, enablePreventDefault: true)]
public static class EventHandlers
{

}


public class ThemeChangedEvent : EventArgs
{
  public bool IsDarkMode { get; set; }
}

public class CalibreMessageEvent : EventArgs
{
  public MessageType? EventType { get; set; }

  public object? Payload { get; set; }

  public Type? PayloadType { get; set; }
  public string? Message { get; set; }

  public Guid? WindowId { get; set; }
}