using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaxCollectData.Library.Business;

internal static class JsonSerializerConfig
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter()  }
    };
}