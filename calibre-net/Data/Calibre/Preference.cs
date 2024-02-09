using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Preference
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public string Val { get; set; } = null!;
}
