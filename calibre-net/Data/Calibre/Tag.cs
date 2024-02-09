using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Link { get; set; } = null!;
}
