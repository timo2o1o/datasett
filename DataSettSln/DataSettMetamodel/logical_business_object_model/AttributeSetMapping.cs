using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This represents the connection between a physical source attribute and a business object.
    /// </summary>
    public class AttributeSetMapping : AttributeSetMappingBase
    {
        public AttributeSetMapping()
        {
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to source attribute
        /// </summary>
        public SourceAttribute? SourceAttribute { get; set; }

        /// <summary>
        /// Navigation property to attribute set
        /// </summary>
        [JsonIgnore]
        public AttributeSet? AttributeSet { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static AttributeSetMapping FromDTO(AttributeSetMappingDTO dto, AttributeSet attributeSet, SourceAttribute srcAttribute)
        {
            return new AttributeSetMapping
            {
                AttributeSet = attributeSet,
                OrderNo = dto.OrderNo,
                HistoryType = dto.HistoryType,
                Role = dto.Role,
                AttributeProperties = dto.AttributeProperties,
                SourceAttribute = srcAttribute
            };
        }
    }
}
