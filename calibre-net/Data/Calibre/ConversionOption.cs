using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class ConversionOption
{
    public int Id { get; set; }

    public string Format { get; set; } = null!;

    public int? Book { get; set; }

    public byte[] Data { get; set; } = null!;
}
