using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class TagBrowserTag
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public int? Count { get; set; }

    public double? AvgRating { get; set; }

    public string? Sort { get; set; }
}
