using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Source Interface represents a specific interface (like a table, file, API endpoint) within a Source System from which data can be extracted.
    /// </summary>
    public class SourceInterface
    {

        // Identification Properties
        // This actually also serves as connection to SourceSystem
        [JsonPropertyName("sourceInterfaceId")]
        public string? SourceInterfaceId { get; set; }

        // Context Properties
        [JsonPropertyName("schema")]
        public string? Schema { get; set; }

        [JsonPropertyName("catalog")]
        public string? Catalog { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("sourceAttributes")]
        public IList<SourceAttribute>? SourceAttributes { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public SourceSystem? ParentSourceSystem { get; set; }

    }
}
