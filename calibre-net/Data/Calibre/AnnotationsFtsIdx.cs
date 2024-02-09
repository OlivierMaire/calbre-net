using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class AnnotationsFtsIdx
{
    public byte[] Segid { get; set; } = null!;

    public byte[] Term { get; set; } = null!;

    public byte[]? Pgno { get; set; }
}
