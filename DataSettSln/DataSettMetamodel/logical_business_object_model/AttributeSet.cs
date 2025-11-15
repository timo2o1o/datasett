using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// An attribute set is a grouping of attributes of a business object.
    /// A possible usage is satellite splitting in data vault models.
    /// </summary>
    public class AttributeSet : AttributeSetBase
    {
        
        public AttributeSet()
        {
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent business object
        /// </summary>
        [JsonIgnore]
        public BusinessObject? BusinessObject { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static AttributeSet FromDTO(AttributeSetDTO dto, BusinessObject parentBusinessObject)
        {
            return new AttributeSet
            {
                Name = dto.Name,
                BusinessObject = parentBusinessObject
            };
        }
    }
}
