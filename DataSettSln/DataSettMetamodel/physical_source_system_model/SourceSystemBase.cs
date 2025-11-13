using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{

    public abstract class SourceSystemBase
    {

        /// <summary>
        /// Database driver or connector type
        /// </summary>
        [JsonPropertyName("driver")]
        public string? Driver { get; set; }

        /// <summary>
        /// Connection string to the source system
        /// </summary>
        [JsonPropertyName("connectionString")]
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Server name or address
        /// </summary>
        [JsonPropertyName("server")]
        public string? Server { get; set; }

        /// <summary>
        /// Name of the source system
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Version of the source system
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }

    }

}