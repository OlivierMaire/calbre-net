using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class TagBrowserRating
{
    public int? Id { get; set; }

    public int? Rating { get; set; }

    public int? Count { get; set; }

    public double? AvgRating { get; set; }

    public int? Sort { get; set; }
}
