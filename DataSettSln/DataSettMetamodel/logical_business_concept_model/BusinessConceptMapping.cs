using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This represents the connection between a physical source attribute and a business object.
    /// </summary>
    public class BusinessConceptMapping : BusinessConceptMappingBase
    {
        public BusinessConceptMapping()
        {
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to source attribute
        /// </summary>
        public SourceAttribute? SourceAttribute { get; set; }

        [JsonIgnore]
        public BusinessConcept? ParentBusinessConcept { get; set; }

        public static BusinessConceptMapping FromSourceAttribute(SourceAttribute srcAttribute)
        {
            return new BusinessConceptMapping
            {
                MappingProperties = srcAttribute.AttributeProperties.Copy(),
                HarmonizedName = srcAttribute.Name,
                HistoryType = Metamodel.HistoryType.None,
                OrderNo = null,
                Role = SourceAttributeRole.Unclassified,
                SourceAttribute = srcAttribute
            };
        }

        public BusinessConceptKeyPart? AssignedKeyPart { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessConceptMapping FromDTO(BusinessConceptMappingDTO dto, BusinessConcept parentBC, SourceAttribute srcAttribute)
        {
            return new BusinessConceptMapping
            {
                ParentBusinessConcept = parentBC,
                AssignedKeyPart = parentBC.KeyParts.Where(kp => kp.Name == dto.BusinessConceptKeyPartName).FirstOrDefault(),
                OrderNo = dto.OrderNo,
                HistoryType = dto.HistoryType,
                Role = dto.Role,
                MappingProperties = dto.MappingProperties,
                SourceAttribute = srcAttribute,
                HarmonizedName = dto.HarmonizedName,
                AttributeSetName = dto.AttributeSetName
            };
        }

        public static BusinessConceptMappingDTO ToDTO(BusinessConceptMapping bcm, string businessConceptId)
        {
            return new BusinessConceptMappingDTO
            {
                BusinessConceptId = businessConceptId,
                BusinessConceptKeyPartName = bcm.AssignedKeyPart?.Name,
                OrderNo = bcm.OrderNo,
                HistoryType = bcm.HistoryType,
                Role = bcm.Role,
                MappingProperties = bcm.MappingProperties,
                AttributeSetName = bcm.AttributeSetName,
                HarmonizedName = bcm.HarmonizedName,
                SourceAttributeName = bcm.SourceAttribute?.Name,
                SourceInterfaceId = bcm.SourceAttribute?.ParentSourceInterface?.ToString()
            };
        }
    }
}
