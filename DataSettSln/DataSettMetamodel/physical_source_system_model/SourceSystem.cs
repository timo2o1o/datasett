using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Represents a physical source system, like a database or a file system,
    /// from which data can be extracted.
    /// </summary>
    public class SourceSystem
    {

        // Identification Properties
        [JsonPropertyName("sourceSystemId")]
        public string? SourceSystemId { get; set; }

        // Context Properties
        [JsonPropertyName("driver")]
        public string? Driver { get; set; }

        [JsonPropertyName("connectionString")]
        public string? ConnectionString { get; set; }

        [JsonPropertyName("server")]
        public string? Server { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public IList<SourceInterface>? SourceInterfaces { get; set; }
    }
}
