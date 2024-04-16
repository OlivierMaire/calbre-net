using Calibre_net.Data.Calibre;
using Calibre_net.Services;
using Calibre_net.Shared.Contracts;
using Dapper;
using FastEndpoints;

namespace Calibre_net.Api.Endpoints;
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
        {SearchTermsConstants.AUTHOR_TAG, "select name from authors where id = @id"},
        {SearchTermsConstants.SERIES_TAG, "select name from series where id = @id"},
        {SearchTermsConstants.RATING_TAG, "select rating as name from ratings where id = @id"},
        {SearchTermsConstants.TAG_TAG, "select name from tags where id = @id"},
        {SearchTermsConstants.PUBLISHER_TAG, "select name from publishers where id = @id"},
        {SearchTermsConstants.LANGUAGE_TAG, "select lang_code as name from languages where id = @id"},
    };

    public override void Configure()
    {
        Post("searchValue");
        Version(1);
        Group<Search>();
        Policies(PermissionType.BOOK_VIEW);
        // ResponseCache((int)TimeSpan.FromDays(1).TotalSeconds,  varyByHeader: "x-request-hash");
         Options(x => x.CacheOutput(p => p.AddPolicy(typeof(MyCustomPolicy))
        .SetVaryByHeader("x-request-hash")
        .Expire(TimeSpan.FromDays(1)) ));
    }

    public override async Task HandleAsync(GetSearchValuesRequest req, CancellationToken ct)
    {
        foreach (var term in req.Terms)
        {
            if (term is not IdSearchTerm)
            // if (string.IsNullOrEmpty(term.Value))
                continue;
            if (!TableMapper.ContainsKey(term.Key))
                continue;

            var tableQuery = TableMapper[term.Key];
            using (var ctx = _dbContext.ConnectionCreate())
            {
                var name = ctx.QueryFirstOrDefault<string>(tableQuery, new { id = term.Value });
                ((IdSearchTerm)term).ValueDisplayName = name ?? string.Empty;
            }
        }

        await SendOkAsync(new GetSearchValuesResponse(req.Terms));
    }
}