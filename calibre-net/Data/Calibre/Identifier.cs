using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Identifier
{
    public int Id { get; set; }

    public int Book { get; set; }

    public string Type { get; set; } = null!;

    public string Val { get; set; } = null!;
}
