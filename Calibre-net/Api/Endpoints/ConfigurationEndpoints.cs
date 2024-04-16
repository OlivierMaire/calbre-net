using Calibre_net.Services;
using Calibre_net.Shared.Contracts;
using FastEndpoints;
using FluentValidation.Results;


public class Configuration : Group{
    public Configuration()
    {
        Configure("configuration", ep => ep.Description(x => x.WithGroupName("configuration")));
    }
}

public sealed class GetCalibreConfigurationEndpoint(ConfigurationService configurationService) : EndpointWithoutRequest<CalibreConfiguration>
{
    private readonly ConfigurationService configurationService = configurationService;
    public override void Configure()
    {
        Get("/");
        Version(1);
        Group<Configuration>();
        Policies(PermissionType.CONFIGURATION_VIEWALL);
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
        Policies(PermissionType.CONFIGURATION_EDIT);
    }

    public override async Task HandleAsync(CalibreConfiguration req, CancellationToken ct)
    {
        configurationService.SetCalibreConfiguration(req);
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
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetConfigurationValueRequest req, CancellationToken ct)
    {
        // return await SendOkAsync(configurationService.GetConfigurationValue(req));
        var response = new GetConfigurationValueResponse(req.Value, configurationService.GetConfigurationValue(req.Value));
         await SendOkAsync(response, ct);

    }
}
