using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This is used to define transformations of source attributes,
    /// like key concatenation, data type conversion, etc.
    /// </summary>
    public class Transformation
    {
        [JsonPropertyName("sourceInterfaceId")]
        public string? SourceInterfaceId { get; set; }

        [JsonPropertyName("sourceAttribute")]
        public string? SourceAttributeName { get; set; }

        [JsonPropertyName("transformation")]
        public string? TransformationExpression { get; set; }
    }
}
