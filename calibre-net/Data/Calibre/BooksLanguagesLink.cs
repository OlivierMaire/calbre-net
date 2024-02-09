using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class BooksLanguagesLink
{
    public int Id { get; set; }

    public int Book { get; set; }

    public int LangCode { get; set; }

    public int ItemOrder { get; set; }
}
