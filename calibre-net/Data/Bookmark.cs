using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calibre_net.Data;

[Table("Bookmarks")]
public class Bookmark
{
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser User { get; set; } = null!;

    public uint BookId {get;set;} 

    [MaxLength(5)]
    public string Format {get;set;} = string.Empty;

    [MaxLength(50)]
    public string Position {get;set;} = string.Empty;

}