namespace calibre_net.Shared.Contracts;

public record SearchRequest{

    public int? Author {get;set;}
    public int? Series {get;set;}
    public int? Rating {get;set;}
    public int? RatingValue {get;set;}
    public SqlOperator RatingValueOperator {get;set;} = SqlOperator.Equals;
    public string? Keyword {get;set;}
    public int? Tag { get; set; }
    public int? Publisher { get; set; }
    public int? Language { get; set; }
    public string? Format { get; set; }
}

public enum SqlOperator
{
    Equals,
    GreaterThan,
    LesserThan,
    GreaterOrEquals,
    LesserOrEquals,
}