
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using calibre_net.Client.Services;
using calibre_net.Data;
using calibre_net.Shared.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Namotion.Reflection;

namespace calibre_net.Services;

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

    public  CalibreConfiguration SetCalibreConfiguration(CalibreConfiguration model)
    {
        try
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "customsettings.json");
            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "{ \"calibre\": {} }");
            string json = File.ReadAllText(filePath);
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

            // configuration.GetSection("calibre").Value = jsonObj["calibre"].ToJsonString();
            // var sectionPath = key.Split(":")[0];

            // if (!string.IsNullOrEmpty(sectionPath))
            // {
            //     var keyPath = key.Split(":")[1];
            //     if (jsonObj[sectionPath] == null)
            //         jsonObj[sectionPath] = JsonNode.Parse("{}");
            //     // if(!jsonObj.HasProperty(sectionPath))
            //     // jsonObj.
            //     jsonObj[sectionPath][keyPath] = JsonValue.Create(value);
            // }
            // else
            // {
            //     jsonObj[sectionPath] = JsonValue.Create(value); // if no sectionpath just set the value
            // }


            var jsonOptions = new System.Text.Json.JsonSerializerOptions()
            {
                WriteIndented = true,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            string output = jsonObj.ToJsonString(jsonOptions);
            File.WriteAllText(filePath, output);
        }
        catch (System.Configuration.ConfigurationErrorsException)
        {
            Console.WriteLine("Error writing app settings");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

        }

        return  GetCalibreConfiguration();
    }


    public string GetConfigurationValue(string key)
    {
        return configuration[key] ?? string.Empty;
    }

}
