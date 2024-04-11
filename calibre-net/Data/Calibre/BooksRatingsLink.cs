using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class BooksRatingsLink
{
    public int Id { get; set; }

    public int Book { get; set; }

    public int Rating { get; set; }
}
