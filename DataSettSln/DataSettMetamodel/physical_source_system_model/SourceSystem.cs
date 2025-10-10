using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class SourceSystem
    {
        [JsonPropertyName("driver")]
        public string? Driver { get; set; } = default!;

        [JsonPropertyName("connectionString")]
        public string? ConnectionString { get; set; } = default!;

        [JsonPropertyName("sourceSystemId")]
        public string? SourceSystemId { get; set; } = default!;

        [JsonPropertyName("server")]
        public string? Server { get; set; } = default!;

        [JsonPropertyName("name")]
        public string? Name { get; set; } = default!;

        [JsonPropertyName("version")]
        public string? Version { get; set; } = default!;

        [JsonIgnore]
        public IList<SourceInterface>? SourceInterfaces { get; set; } = default!;
    }
}
