using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for BusinessObjectRelationItem.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// </summary>
    public class BusinessObjectRelationItemDTO : BusinessObjectRelationItemBase
    {
        [JsonConstructor]
        public BusinessObjectRelationItemDTO()
        {
        }

        /// <summary>
        /// Foreign key reference to related key (business object)
        /// </summary>
        [JsonPropertyName("relatedKey")]
        public string? RelatedKeyId { get; set; }
    }
}
