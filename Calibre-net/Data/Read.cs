using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calibre_net.Data;

[Table("ReadStates")]
public class Read
{
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser User { get; set; } = null!;

    public uint BookId { get; set; }

    public bool MarkedAsRead {get; set; }

}