using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for AttributeSet.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// </summary>
    public class AttributeSetDTO : AttributeSetBase
    {
        [JsonConstructor]
        public AttributeSetDTO()
        {
        }

        public AttributeSetDTO(string name, string? businessObjectId = null)
        {
            Name = name;
            BusinessObjectId = businessObjectId;
            if (!string.IsNullOrEmpty(businessObjectId) && !string.IsNullOrEmpty(name))
            {
                Id = string.Format("{0}.{1}", businessObjectId, name);
            }
        }

        /// <summary>
        /// Foreign key reference to parent business object
        /// </summary>
        [JsonPropertyName("businessObjectId")]
        public string? BusinessObjectId { get; set; }
    }
}
