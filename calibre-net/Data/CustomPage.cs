using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Calibre_net.Data;

[Table("Pages")]
[Index(nameof(Slug), IsUnique = true)]
public class CustomPage
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? OwnerId { get; set; } = null;
    public virtual ApplicationUser? Owner { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    public bool Public { get; set; } = false;

    public string Slug { get; set; } = string.Empty;

    public int OrderPosition { get; set; } = 0;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

}