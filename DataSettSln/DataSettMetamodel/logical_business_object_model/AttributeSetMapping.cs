using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This represents the connection between a physical source attribute and
    /// a business object.
    /// </summary>
    public class AttributeSetMapping
    {
        
        // Identification Properties
        [JsonPropertyName("attributeSetId")]
        public string? AttributeSetId { get; set; }

        [JsonPropertyName("sourceInterfaceId")]
        public string? SourceInterfaceId { get; set; }

        [JsonPropertyName("sourceAttributeName")]
        public string? SourceAttributeName { get; set; }
        
        // Context Properties
        [JsonPropertyName("orderNo")]
        public int? OrderNo { get; set; }

        [JsonPropertyName("historyType")]
        public HistoryType? HistoryType { get; set; }

        [JsonPropertyName("role")]
        public SourceAttributeRole? Role { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("default")]
        public string? Default { get; set; }

        [JsonPropertyName("nullable")]
        public bool? Nullable { get; set; }

        [JsonPropertyName("datatype")]
        public string? Datatype { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("precision")]
        public int? Precision { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public SourceAttribute? SourceAttribute { get; set; }

        [JsonIgnore]
        public AttributeSet? AttributeSet { get; set; }
        
    }
}
