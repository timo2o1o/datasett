using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataSett.Metamodel.Serde;

/// <summary>
/// Provides shared, reusable JsonSerializerOptions configurations for consistent serialization
/// across the DataSettMetamodelSerde library.
/// </summary>
/// <remarks>
/// This class ensures that all JSON serialization/deserialization operations use consistent
/// settings, particularly camelCase naming conventions for properties and dictionary keys.
/// </remarks>
public static class JsonDefaults
{
    /// <summary>
    /// Gets a shared JsonSerializerOptions instance configured with web-friendly defaults
    /// and camelCase naming conventions.
    /// </summary>
    /// <remarks>
    /// This instance is configured with:
    /// - JsonSerializerDefaults.Web: Provides sensible defaults for web scenarios
    /// - PropertyNamingPolicy: JsonNamingPolicy.CamelCase for property names
    /// - DictionaryKeyPolicy: JsonNamingPolicy.CamelCase for dictionary keys
    /// 
    /// The options are created once and reused to avoid unnecessary allocations and ensure
    /// consistent behavior across all serialization operations.
    /// </remarks>
    public static JsonSerializerOptions Web { get; } = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
}
