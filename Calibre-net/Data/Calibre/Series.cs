using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Series
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Sort { get; set; }

    public string Link { get; set; } = null!;
}
