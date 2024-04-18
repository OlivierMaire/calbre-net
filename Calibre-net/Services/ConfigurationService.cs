
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using Calibre_net.Client.Services;
using Calibre_net.Data;
using Calibre_net.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Namotion.Reflection;

namespace Calibre_net.Services;

[ScopedRegistration]
public class ConfigurationService(ApplicationDbContext dbContext,
ILogger<ConfigurationService> logger,
IOptionsSnapshot<CalibreConfiguration> configSnap,
IConfiguration configuration)
{
    private readonly ApplicationDbContext dbContext = dbContext;
    private readonly ILogger<ConfigurationService> logger = logger;
    private readonly IConfiguration configuration = configuration;
    // private readonly IOptionsMonitor<CalibreConfiguration> configMonitor = configMonitor;
    // private readonly IOptions<CalibreConfiguration> configOptions = configOptions;
    private readonly IOptionsSnapshot<CalibreConfiguration> configSnap = configSnap;
    // private readonly IOptions<ServerConfiguration> serverOptions = serverOptions;


    public CalibreConfiguration GetCalibreConfiguration()
    {
        return configSnap.Value;
    }

    public CalibreConfiguration SetCalibreConfiguration(CalibreConfiguration model)
    {
        try
        {
            logger.LogInformation("Saving Calibre Configuration");

            var filePath = Path.Combine(AppContext.BaseDirectory, "customsettings.json");

            logger.LogInformation($"Looking for file: {filePath}");


            if (!File.Exists(filePath))
            {
                logger.LogInformation($"No file found, writing defaults");

                File.WriteAllText(filePath, "{ \"calibre\": {} }");
            }
            string json = File.ReadAllText(filePath);

            logger.LogInformation($"Reading File");
            logger.LogInformation(json);

            // dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            // dynamic? jsonObj = System.Text.Json.JsonSerializer.Deserialize<dynamic>(json);
            // JsonElement? jsonObj = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(json);
            var jsonObj = JsonNode.Parse(json);

            if (jsonObj == null)
                return new CalibreConfiguration();

            if (jsonObj["calibre"] == null)
                jsonObj["calibre"] = JsonNode.Parse("{}");
            // var calibreNode = jsonObj["calibre"];

            var modelJson = System.Text.Json.JsonSerializer.Serialize(model);

            jsonObj["calibre"] = JsonNode.Parse(modelJson);

            var jsonOptions = new System.Text.Json.JsonSerializerOptions()
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            string output = jsonObj.ToJsonString(jsonOptions);

            logger.LogInformation($"Updated config");
            logger.LogInformation(output);

            logger.LogInformation($"Writing to {filePath}");

            File.WriteAllText(filePath, output);
        }
        catch (System.Configuration.ConfigurationErrorsException e)
        {
            logger.LogError(e, "Error writing app settings");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception");

        }

        return GetCalibreConfiguration();
    }


    public string GetConfigurationValue(string key)
    {
        return configuration[key] ?? string.Empty;
    }

}
