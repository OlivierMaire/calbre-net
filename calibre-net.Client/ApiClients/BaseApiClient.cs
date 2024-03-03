namespace calibre_net.Client.ApiClients;
using calibre_net.Client.Services;

// [SingletonRegistration]
public class BaseApiClient
{
    private readonly IHttpClientFactory httpClientFactory;

    public BaseApiClient(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public Task<HttpClient> CreateHttpClientAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(this.httpClientFactory.CreateClient("calibre-net.Api"));
    }


        static protected void UpdateJsonSerializerSettings(System.Text.Json.JsonSerializerOptions settings){
            settings.PropertyNameCaseInsensitive = true;
        }

}

// [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.0.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
// public partial class ProblemDetails
// {

//     [System.Text.Json.Serialization.JsonPropertyName("type")]
//     public string Type { get; set; }

//     [System.Text.Json.Serialization.JsonPropertyName("title")]
//     public string Title { get; set; }

//     [System.Text.Json.Serialization.JsonPropertyName("status")]
//     public int? Status { get; set; }

//     [System.Text.Json.Serialization.JsonPropertyName("detail")]
//     public string Detail { get; set; }

//     [System.Text.Json.Serialization.JsonPropertyName("instance")]
//     public string Instance { get; set; }

//     private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

//     [System.Text.Json.Serialization.JsonExtensionData]
//     public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
//     {
//         get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
//         set { _additionalProperties = value; }
//     }

// }