using System.Collections.Generic;
using calibre_net.Client.Services;
using calibre_net.Shared.Contracts;
using Calibre_net.Data.Calibre;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Z.EntityFramework.Plus;

namespace calibre_net.Services;

[ScopedRegistration]
public class CustomColumnService(CalibreDbContext calibreDb)

{
    private readonly CalibreDbContext calibreDb = calibreDb;

    public List<CustomColumn> GetCustomColumns()
    {
        return calibreDb.CustomColumns.FromCache("custom_columns").ToList();
    }

    public Dictionary<int, DbSet<GenericCustomColumn>> GetCustomColumnTables()
    {
        var customColumns = this.GetCustomColumns();
        var tableTypeDict = customColumns.ToDictionary(cc => cc.Id, cc =>
calibreDb.Set<GenericCustomColumn>($"custom_column_{cc.Id}"));


        return tableTypeDict;
    }

}