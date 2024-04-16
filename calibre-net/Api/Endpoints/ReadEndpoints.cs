using Calibre_net.Data;
using Calibre_net.Services;
using Calibre_net.Shared.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using System.Security.Claims;

namespace Calibre_net.Api.Endpoints;

public class ReadGroup : Group
{
    public ReadGroup()
    {
        Configure("read", ep =>
        {
            ep.Description(x => x.WithGroupName("read"));
            ep.EndpointVersion(1);
        });
    }
}

public sealed class SetReadStatusEndpoint(ApplicationDbContext dbContext, IOutputCacheStore cachePolicy) : Endpoint<SetReadStatusRequest, bool>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IOutputCacheStore _cachePolicy = cachePolicy;

    public override void Configure()
    {
        Put("/{bookId}");
        Group<ReadGroup>();
    }

    public override async Task HandleAsync(SetReadStatusRequest req, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var state = await _dbContext.ReadStates.FindAsync([userId, (uint)req.BookId], ct);
        if (state == null)
        {
            state = new Read
            {
                BookId = (uint)req.BookId,
                UserId = userId
            };
            _dbContext.ReadStates.Add(state);
        }
        state.MarkedAsRead = req.Status;
        await _dbContext.SaveChangesAsync(ct);
        await _cachePolicy.EvictByTagAsync("book_search",ct);
        await SendOkAsync(state.MarkedAsRead, ct);
    }
}