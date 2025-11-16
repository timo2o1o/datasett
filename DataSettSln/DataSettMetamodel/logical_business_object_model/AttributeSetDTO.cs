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

        [JsonIgnore]
        public string? AttributeSetId
        {
            get
            {
                return $"{BusinessObjectId}.{Name}";
            }
        }

        /// <summary>
        /// Foreign key reference to parent business object
        /// </summary>
        public string? BusinessObjectId { get; set; }
    }
}
