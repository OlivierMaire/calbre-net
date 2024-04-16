using System.Text.Json;
using System.Text.Json.Serialization;

namespace Calibre_net.Shared;

public class SensibleDataConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        // do not write anything
        writer.WriteNullValue();
    }
}