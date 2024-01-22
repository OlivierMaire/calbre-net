using System.Text.Json;
using System.Text.Json.Serialization;

namespace calibre_net.Shared;

public class MudCheckboxConverter : JsonConverter<bool>
{
    public override bool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>
            reader.GetString() == "on";


    public override void Write(
        Utf8JsonWriter writer,
        bool booleanValue,
        JsonSerializerOptions options) =>
            writer.WriteStringValue(booleanValue ? "true" : "false");
}