using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Rating
{
    public int Id { get; set; }

    public int? RatingValue { get; set; }

    public string Link { get; set; } = null!;
}
