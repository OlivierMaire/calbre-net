using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class BooksAuthorsLink
{
    public int Id { get; set; }

    public int Book { get; set; }

    public int Author { get; set; }
}
