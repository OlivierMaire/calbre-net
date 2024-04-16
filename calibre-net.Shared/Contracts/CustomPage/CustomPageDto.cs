namespace Calibre_net.Shared.Contracts;

public partial class CustomPageDto
{
    
    public int Id { get; init; }

    public string? OwnerId { get; init; } = null;
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    public bool Public { get; set; } = false;

    public string Slug {get;init;} = string.Empty;
    public int OrderPosition { get; set; } = 0;

    public DateTimeOffset CreatedAt {get;init;}
    public DateTimeOffset UpdatedAt {get;set;}
}