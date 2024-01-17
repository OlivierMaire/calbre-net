
using calibre_net.Client.Services;
using calibre_net.Data;
using calibre_net.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace calibre_net.Services;

[ScopedRegistration]
public class ConfigurationService(ApplicationDbContext dbContext,
ILogger<ConfigurationService> logger)
{
    private readonly ApplicationDbContext dbContext = dbContext;
    private readonly ILogger<ConfigurationService> logger = logger;

    public async Task<DatabaseConfiguration> GetDatabaseConfigurationAsync()
    {
        var config = dbContext.Configurations.Where(c => c.Category == CalibreConfiguration.DatabaseConfiguration.CATEGORY_NAME);
        DatabaseConfiguration model = new DatabaseConfiguration();
        model.Location = (await config.FirstOrDefaultAsync(c =>
            c.Key == CalibreConfiguration.DatabaseConfiguration.LOCATION))?.Value ?? string.Empty;

        return model;
    }

    public async Task<DatabaseConfiguration> SetDatabaseConfigurationAsync(DatabaseConfiguration model)
    {
        var config = dbContext.Configurations.Where(c => c.Category == CalibreConfiguration.DatabaseConfiguration.CATEGORY_NAME);
        var location = await config.FirstOrDefaultAsync(c => c.Key == CalibreConfiguration.DatabaseConfiguration.LOCATION);

        if (location != null)
        {
            location.Value = model.Location;
        }
        else
        {
            location = new CalibreConfiguration();
            location.Value = model.Location;
            dbContext.Configurations.Add(location);
        }

        await dbContext.SaveChangesAsync();

        return await GetDatabaseConfigurationAsync();
    }

}
