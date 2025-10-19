using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class AttributeSetMapping
    {
        [JsonConstructor]
        public AttributeSetMapping()
        {

        }

        [JsonPropertyName("attributeSetId")]
        public string? AttributeSetId { get; set; }

        [JsonPropertyName("sourceInterfaceId")]
        public string? SourceInterfaceId { get; set; }

        [JsonPropertyName("orderNo")]
        public int? OrderNo { get; set; }

        [JsonPropertyName("sourceAttributeName")]
        public string? SourceAttributeName { get; set; }

        [JsonIgnore]
        public SourceAttribute? SourceAttribute { get; set; }

        [JsonPropertyName("historyType")]
        public HistoryType? HistoryType { get; set; }

        [JsonPropertyName("role")]
        public SourceAttributeRole? Role { get; set; }

        [JsonPropertyName("relation")]
        public string? Relation { get; set; }
        
        [JsonIgnore]
        public BusinessObjectRelation? RelatedRelation { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; } = default!;

        [JsonPropertyName("default")]
        public string? Default { get; set; } = "";

        [JsonPropertyName("nullable")]
        public bool? Nullable { get; set; } = default!;

        [JsonPropertyName("datatype")]
        public string? Datatype { get; set; } = "";

        [JsonPropertyName("length")]
        public int? Length { get; set; } = default!;
    }
}
