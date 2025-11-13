using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Base class for SourceInterface containing scalar and context properties.
    /// This class is part of the Base/DTO/Domain separation pattern.
    /// Source Interface represents a specific interface (like a table, file, API endpoint) 
    /// within a Source System from which data can be extracted.
    /// </summary>
    public abstract class SourceInterfaceBase
    {
        /// <summary>
        /// Unique identifier for the source interface
        /// </summary>
        [JsonPropertyName("sourceInterfaceId")]
        public string? SourceInterfaceId { get; set; }

        /// <summary>
        /// Schema name (for databases)
        /// </summary>
        [JsonPropertyName("schema")]
        public string? Schema { get; set; }

        /// <summary>
        /// Catalog name (for databases)
        /// </summary>
        [JsonPropertyName("catalog")]
        public string? Catalog { get; set; }

        /// <summary>
        /// Name of the interface (table, file, etc.)
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
