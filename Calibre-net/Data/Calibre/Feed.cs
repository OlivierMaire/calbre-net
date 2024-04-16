using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Feed
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Script { get; set; } = null!;
}
