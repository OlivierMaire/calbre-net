using System.Security.Claims;
using Calibre_net.Data;
using Calibre_net.Shared.Contracts;
using FastEndpoints;
using Markdig;
using Calibre_net.Shared;
using Microsoft.EntityFrameworkCore;

namespace Calibre_net.Api.Endpoints;
public class CustomPageGroup : Group
{
    public CustomPageGroup()
    {
        Configure("page", ep =>
        {
            ep.Description(x => x.WithGroupName("page"));
            ep.EndpointVersion(1);
            ep.AllowAnonymous();
        });
    }
}

public sealed class GetAllPagesEndpoint(ApplicationDbContext dbContext) : EndpointWithoutRequest<GetAllCustomPagesResponse>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public override void Configure()
    {
        Get("/all");
        Group<CustomPageGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var pages = _dbContext.CustomPages.Where(cp => cp.OwnerId == userId).ProjectToDto();
        await SendOkAsync(new GetAllCustomPagesResponse([.. pages]), ct);
    }
}

public sealed class PutCustomPageEndpoint(ApplicationDbContext dbContext) : Endpoint<PutCustomPageRequest, CustomPageDto>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public override void Configure()
    {
        Put("/");
        Group<CustomPageGroup>();
    }

    public override async Task HandleAsync(PutCustomPageRequest req, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        CustomPage? pageDb;
        if (req.Page.Id != 0)
        {
            pageDb = _dbContext.CustomPages.Find(req.Page.Id);
            if (pageDb?.OwnerId != userId)
                await SendUnauthorizedAsync(ct);
        }
        else
        {
            pageDb = new()
            {
                CreatedAt = DateTimeOffset.UtcNow,
                OwnerId = userId,
            };
            _dbContext.CustomPages.Add(pageDb);
        }
        if (pageDb != null)
        {
            pageDb.Public = req.Page.Public;
            pageDb.Title = req.Page.Title;
            pageDb.Content = req.Page.Content;
            pageDb.OrderPosition = req.Page.OrderPosition;
            pageDb.UpdatedAt = DateTimeOffset.UtcNow;
            var slug = req.Page.Title.GenerateSlug();
            pageDb.Slug = slug;
            int slugNum = 1;
            // validate Slug uniqueness
            while (await _dbContext.CustomPages.AnyAsync(x => x.Slug == pageDb.Slug && x.Id != pageDb.Id, ct))
            {
                slugNum++;
                pageDb.Slug = slug[..^(slugNum.ToString().Length + 1)] + "-" + slugNum.ToString();
            }


            await _dbContext.SaveChangesAsync(ct);


            await SendOkAsync(pageDb.ToDto(), ct);
            return;
        }

        await SendErrorsAsync(cancellation: ct);

    }


}

public sealed class GetCustomPageEndpoint(ApplicationDbContext dbContext) : Endpoint<GetCustomPageRequest, CustomPageDto>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public override void Configure()
    {
        Get("/{id}");
        Group<CustomPageGroup>();
    }

    public override async Task HandleAsync(GetCustomPageRequest req, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var pageDb = await _dbContext.CustomPages.FindAsync([req.Id], ct);
        if (pageDb?.OwnerId != userId)
            await SendUnauthorizedAsync(ct);
        if (pageDb != null)
            await SendOkAsync(pageDb.ToDto(), ct);
        else
            await SendNotFoundAsync(ct);
    }
}

public sealed class DeleteCustomPageEndpoint(ApplicationDbContext dbContext) : Endpoint<DeleteCustomPageRequest, bool>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public override void Configure()
    {
        Delete("/{id}");
        Group<CustomPageGroup>();
    }

    public override async Task HandleAsync(DeleteCustomPageRequest req, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var pageDb = await _dbContext.CustomPages.FindAsync([req.Id], ct);
        if (pageDb?.OwnerId != userId)
            await SendUnauthorizedAsync(ct);
        if (pageDb != null)
        {
            _dbContext.CustomPages.Remove(pageDb);
            await _dbContext.SaveChangesAsync(ct);
        }

        await SendOkAsync(true, ct);
    }
}


public sealed class GetCustomPageMarkupEndpoint(ApplicationDbContext dbContext) : Endpoint<GetCustomPageMarkupRequest, GetCustomPageMarkupResponse>
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public override void Configure()
    {
        Get("/markup/{slug}");
        Group<CustomPageGroup>();
    }

    public override async Task HandleAsync(GetCustomPageMarkupRequest req, CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var pageDb = await _dbContext.CustomPages.SingleAsync(x => x.Slug == req.Slug 
        && (x.OwnerId == userId || x.Public), ct);
        if (pageDb == null)
            await SendNotFoundAsync(ct);
        // Configure the pipeline with all advanced extensions active
        var pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions().Build();
        var markup = Markdig.Markdown.ToHtml(pageDb?.Content ?? string.Empty, pipeline: pipeline);

    Console.WriteLine(markup);

        await SendOkAsync(new GetCustomPageMarkupResponse(markup), ct);
    }
}

public sealed class GetCustomPagesLinksEndpoint(ApplicationDbContext dbContext) : EndpointWithoutRequest<GetCustomPagesLinksResponse[]>
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public override void Configure()
    {
        Get("/links");
        Group<CustomPageGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        var slugs = _dbContext.CustomPages.Where(cp => cp.OwnerId == userId || cp.Public)
        .OrderBy(cp => cp.OrderPosition)
        .Select(cp => new GetCustomPagesLinksResponse(cp.Title, "/page/" + cp.Slug))
        .ToArray();

        await SendOkAsync(slugs, ct);
    }
}