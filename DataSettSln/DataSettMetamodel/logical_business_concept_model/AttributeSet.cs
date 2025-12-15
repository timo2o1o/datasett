using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// An attribute set is a grouping of attributes of a business concept.
    /// A possible usage is satellite splitting in data vault models.
    /// </summary>
    public class AttributeSet : AttributeSetBase
    {
        
        public AttributeSet()
        {

            AttributeSetMappings = new List<AttributeSetMapping>();
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent business concept
        /// </summary>
        [JsonIgnore]
        public BusinessConcept? ParentBusinessConcept { get; set; }

        public IList<AttributeSetMapping> AttributeSetMappings { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static AttributeSet FromDTO(AttributeSetDTO dto, BusinessConcept parentBusinessConcept, IDictionary<string, SourceAttribute> attributeCache)
        {
            AttributeSet result = new AttributeSet
            {
                Name = dto.Name,
                ParentBusinessConcept = parentBusinessConcept
            };

            foreach (AttributeSetMappingDTO currentMapping in dto.AttributeSetMappings)
            {
                SourceAttribute srcAttribute = attributeCache[$"{currentMapping.SourceInterfaceId}.{currentMapping.SourceAttributeName}"];
                result.AttributeSetMappings.Add(AttributeSetMapping.FromDTO(currentMapping, result, srcAttribute));
            }

            return result;
        }
    }
}
