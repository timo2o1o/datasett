using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class BusinessObjectRelationItem
    {

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }

        [JsonPropertyName("relatedKey")]
        public string? RelatedKeyId { get; set; }

        [JsonPropertyName("isLeadingKey")]
        public bool? IsLeadingKey { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public BusinessObject? RelatedKey { get; set; }


    }
}
