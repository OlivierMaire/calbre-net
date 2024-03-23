namespace calibre_net.Shared.Contracts;

public record SearchRequest{

    public int? Author {get;set;}
    public int? Series {get;set;}
    public int? Rating {get;set;}
    public string? Keyword {get;set;}

}