using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Describes leading key of a relation between business concepts
    /// </summary>
    public class BusinessConceptRelationItem : BusinessConceptRelationItemBase
    {
        /// <summary>
        /// Parent identifier
        /// </summary>
        public BusinessConcept? RelatedBusinessConcept { get; set; }

        public static BusinessConceptRelationItem FromDTO(BusinessConceptRelationItemDTO dto, IDictionary<string, BusinessConcept> businessConceptCache)
        {

            BusinessConceptRelationItem newItem = new BusinessConceptRelationItem
            {
                IsLeadingKey = dto.IsLeadingKey
            };

            if (dto.RelatedBusinessConceptId != null)
            {
                string bcId = dto.RelatedBusinessConceptId;

                if (businessConceptCache.ContainsKey(bcId))
                {
                    newItem.RelatedBusinessConcept = businessConceptCache[dto.RelatedBusinessConceptId];
                }

            }
            
            return newItem;
        }

    }
}