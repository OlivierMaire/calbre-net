using calibre_net.Services;
using calibre_net.Shared.Contracts;
using FastEndpoints;
using FluentValidation.Results;


public class Configuration : Group{
    public Configuration()
    {
        Configure("configuration", ep => ep.Description(x => x.AllowAnonymous().WithGroupName("configuration")));
    }
}

public sealed class GetCalibreConfigurationEndpoint(ConfigurationService configurationService) : EndpointWithoutRequest
{
    private readonly ConfigurationService configurationService = configurationService;
    public override void Configure()
    {
        Get("/");
        Version(1);
        Group<Configuration>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(configurationService.GetCalibreConfiguration(), ct);
    }
}

public sealed class SetCalibreConfigurationEndpoint(ConfigurationService configurationService) : Endpoint<CalibreConfiguration>
{
    private readonly ConfigurationService configurationService = configurationService;
    public override void Configure()
    {
        Post("/");
        Version(1);
        Group<Configuration>();
    }

    public override async Task HandleAsync(CalibreConfiguration req, CancellationToken ct)
    {
        await configurationService.SetCalibreConfigurationAsync(req);
        await SendOkAsync(ct);

    }
}

public sealed class GetConfigurationValueEndpoint(ConfigurationService configurationService) : Endpoint<GetConfigurationValueRequest, GetConfigurationValueResponse>
{
    private readonly ConfigurationService configurationService = configurationService;
    public override void Configure()
    {
        Get("/getvalue");
        Version(1);
        Group<Configuration>();
    }

    public override async Task HandleAsync(GetConfigurationValueRequest req, CancellationToken ct)
    {
        // return await SendOkAsync(configurationService.GetConfigurationValue(req));
        var response = new GetConfigurationValueResponse(req.Value, configurationService.GetConfigurationValue(req.Value));
         await SendOkAsync(response, ct);

    }
}
