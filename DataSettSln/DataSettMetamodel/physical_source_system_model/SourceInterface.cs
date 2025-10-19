using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class SourceInterface
    {
        [JsonPropertyName("sourceInterfaceId")]
        public string? SourceInterfaceId { get; set; } = default!;

        [JsonPropertyName("sourceSystemId")]
        public string? SourceSystemId { get; set; } = default!;

        [JsonPropertyName("schema")]
        public string? Schema { get; set; } = default!;

        [JsonPropertyName("catalog")]
        public string? Catalog { get; set; } = default!;

        [JsonPropertyName("name")]
        public string? Name { get; set; } = default!;

        [JsonPropertyName("sourceAttributes")]
        public IList<SourceAttribute>? SourceAttributes { get; set; } = default!;

    }
}
