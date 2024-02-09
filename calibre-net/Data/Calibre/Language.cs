using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class Language
{
    public int Id { get; set; }

    public string LangCode { get; set; } = null!;

    public string Link { get; set; } = null!;
}
