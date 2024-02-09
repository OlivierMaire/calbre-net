using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class LastReadPosition
{
    public int Id { get; set; }

    public int Book { get; set; }

    public string Format { get; set; } = null!;

    public string User { get; set; } = null!;

    public string Device { get; set; } = null!;

    public string Cfi { get; set; } = null!;

    public double Epoch { get; set; }

    public double PosFrac { get; set; }
}
