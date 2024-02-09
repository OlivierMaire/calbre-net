using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class BooksPluginDatum
{
    public int Id { get; set; }

    public int Book { get; set; }

    public string Name { get; set; } = null!;

    public string Val { get; set; } = null!;
}
