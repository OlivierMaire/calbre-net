using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Datum
{
    public int Id { get; set; }

    public int Book { get; set; }

    public string Format { get; set; } = null!;

    public int UncompressedSize { get; set; }

    public string Name { get; set; } = null!;
}
