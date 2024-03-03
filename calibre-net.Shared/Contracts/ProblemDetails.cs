namespace calibre_net.Shared.Contracts;
 
 
 [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.0.0.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ProblemDetails
    {

        [System.Text.Json.Serialization.JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("title")]
        public string Title { get; set; }= string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("status")]
        public int? Status { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("detail")]
        public string Detail { get; set; }= string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("instance")]
        public string Instance { get; set; }= string.Empty;

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new Dictionary<string, object>();

        [System.Text.Json.Serialization.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
            set { _additionalProperties = value; }
        }

    }