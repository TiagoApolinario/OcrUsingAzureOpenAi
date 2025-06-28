using System.Text.Json;
using System.Text.Json.Schema;

namespace OcrUsingAzureOpenAi.ConsoleApp;

public static class JsonSchemaExporter
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerOptions.Default)
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static readonly JsonSchemaExporterOptions ExporterOptions = new()
    {
        TreatNullObliviousAsNonNullable = true
    };

    public static BinaryData GetJsonSchemaBinaryData<T>()
        where T : class
    {
        var schema = SerializerOptions
            .GetJsonSchemaAsNode(typeof(T), ExporterOptions);

        return BinaryData.FromString(schema.ToString());
    }
}