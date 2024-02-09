﻿using System;
using System.Collections.Generic;

namespace Calibre_net.Data.Calibre;

public partial class CustomColumn
{
    public int Id { get; set; }

    public string Label { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Datatype { get; set; } = null!;

    public bool MarkForDelete { get; set; }

    public bool Editable { get; set; }

    public string Display { get; set; } = null!;

    public bool IsMultiple { get; set; }

    public bool Normalized { get; set; }
}
