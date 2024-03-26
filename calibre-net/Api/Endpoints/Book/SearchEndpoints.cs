using calibre_net.Data.Calibre;
using calibre_net.Services;
using calibre_net.Shared.Contracts;
using Dapper;
using FastEndpoints;

namespace calibre_net.Api.Endpoints;
public class Search : Group
{
    public Search()
    {
        Configure("search", ep => ep.Description(x => x.WithGroupName("search")));
    }
}

public sealed class GetSearchValuesEndpoint(CalibreDbDapperContext dbContext) : Endpoint<GetSearchValuesRequest, GetSearchValuesResponse>
{
    private readonly CalibreDbDapperContext _dbContext = dbContext;
    public static readonly Dictionary<string, string> TableMapper = new Dictionary<string, string>{
        {"author", "select name from authors where id = @id"},
        {"series", "select name from series where id = @id"},
        {"rating", "select rating as name from ratings where id = @id"},
        {"tag", "select name from tags where id = @id"},
        {"publisher", "select name from publishers where id = @id"},
        {"language", "select lang_code as name from languages where id = @id"},
    };

    public override void Configure()
    {
        Post("searchValue");
        Version(1);
        Group<Search>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetSearchValuesRequest req, CancellationToken ct)
    {
        foreach (var term in req.Terms)
        {
            if (string.IsNullOrEmpty(term.Value))
                continue;
            if (!TableMapper.ContainsKey(term.Key))
                continue;
            if (!TableMapper.ContainsKey(term.Key))
                continue;

            var tableQuery = TableMapper[term.Key];
            using (var ctx = _dbContext.ConnectionCreate())
            {
                var name = ctx.QueryFirst<string>(tableQuery, new { id = term.Value });
                term.ValueName = name;
            }
        }



        await SendOkAsync(new GetSearchValuesResponse(req.Terms));
    }
}