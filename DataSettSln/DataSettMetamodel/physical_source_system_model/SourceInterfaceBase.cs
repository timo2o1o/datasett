using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public abstract class SourceInterfaceBase
    {

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

        /// <summary>
        /// List of attributes within the source interface
        /// </summary>
        [JsonPropertyName("sourceAttributes")]
        public IList<SourceAttribute>? SourceAttributes { get; set; }

    }
}
