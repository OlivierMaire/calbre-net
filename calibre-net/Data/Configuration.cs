
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace calibre_net.Data;

[Table("Configuration")]
[PrimaryKey(nameof(Category), nameof(Key))]
public class CalibreConfiguration
{
    public string Category { get; set; } = string.Empty;

    public string Key { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public static class DatabaseConfiguration {
        public const string CATEGORY_NAME = "database";

        public const string LOCATION = "location";
    }

}


