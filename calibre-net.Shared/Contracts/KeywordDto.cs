using System.Text.Json.Serialization;

namespace calibre_net.Shared.Contracts;
public partial class KeywordDto : Searchable
{
    public string Keyword { get; set; } = string.Empty;

    [JsonIgnore]
    public override string SearchUrl => $"keyword/{Keyword}";

}