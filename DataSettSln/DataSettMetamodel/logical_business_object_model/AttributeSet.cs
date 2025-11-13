using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for AttributeSet containing navigation properties and business logic.
    /// Inherits from AttributeSetBase which contains scalar/context properties.
    /// An attribute set is a grouping of attributes of a business object.
    /// A possible usage is satellite splitting in data vault models.
    /// For serialization, use AttributeSetDTO instead.
    /// </summary>
    public class AttributeSet : AttributeSetBase
    {
        [JsonConstructor]
        public AttributeSet()
        {
        }

        public AttributeSet(string name, BusinessObject? businessObject = null)
        {
            BusinessObject = businessObject;
            Name = name;
            Id = string.Format("{0}.{1}", BusinessObject?.Id, Name);
            BusinessObjectId = BusinessObject?.Id;
        }

        /// <summary>
        /// Foreign key reference to business object (for compatibility)
        /// </summary>
        [JsonIgnore]
        public string? BusinessObjectId { get; set; }

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent business object
        /// </summary>
        [JsonIgnore]
        public BusinessObject? BusinessObject { get; set; }

        /// <summary>
        /// Converts this attribute set to a DTO for serialization
        /// </summary>
        public AttributeSetDTO ToDTO()
        {
            return new AttributeSetDTO
            {
                Id = this.Id,
                Name = this.Name,
                BusinessObjectId = this.BusinessObject?.Id
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static AttributeSet FromDTO(AttributeSetDTO dto)
        {
            return new AttributeSet
            {
                Id = dto.Id,
                Name = dto.Name,
                BusinessObjectId = dto.BusinessObjectId
            };
        }
    }
}
