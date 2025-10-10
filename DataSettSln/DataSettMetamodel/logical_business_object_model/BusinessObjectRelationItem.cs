using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class BusinessObjectRelationItem
    {
        [JsonConstructor]
        public BusinessObjectRelationItem()
        {

        }

        public BusinessObjectRelationItem(BusinessObject relatedKey, BusinessObjectRelation relation = null, bool isLeadingKey = false)
        {
            Parent = relation?.Name;
            RelatedKey = relatedKey;
            RelatedKeyId = relatedKey.Id;
            IsLeadingKey = isLeadingKey;
        }


        [JsonIgnore]
        public BusinessObject? RelatedKey{ get; set; }

        [JsonPropertyName("parent")]
        public string? Parent { get; set; }

        [JsonPropertyName("relatedKey")]
        public string? RelatedKeyId { get; set; }

        [JsonPropertyName("isLeadingKey")]
        public bool? IsLeadingKey { get; set; }

    }
}
