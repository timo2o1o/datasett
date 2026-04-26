using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    public class BusinessConceptRelation : BusinessConceptRelationBase
    {

        public BusinessConceptRelation()
        {
            RelatedConcepts = new List<BusinessConceptRelationItem>();
        }

        [JsonIgnore]
        public BusinessDomain? ParentBusinessDomain { get; set; }

        public IList<BusinessConceptRelationItem> RelatedConcepts { get; set; }

        public static BusinessConceptRelation FromDTO(BusinessConceptRelationDTO dto, BusinessDomain parentBusinessDomain, IDictionary<string, BusinessConcept> businessConceptCache)
        {
            BusinessConceptRelation newRelation = new BusinessConceptRelation
            {
                Name = dto.Name,
                ParentBusinessDomain = parentBusinessDomain
            };

            foreach (BusinessConceptRelationItemDTO currentItem in dto.RelatedConcepts)
            {
                newRelation.RelatedConcepts.Add(BusinessConceptRelationItem.FromDTO(currentItem, businessConceptCache));
            }

            return newRelation;
        }

        public static BusinessConceptRelationDTO ToDTO(BusinessConceptRelation relation, string businessDomainId)
        {
            BusinessConceptRelationDTO dto = new BusinessConceptRelationDTO
            {
                Name = relation.Name,
                BusinessDomainId = businessDomainId
            };

            foreach (BusinessConceptRelationItem currentItem in relation.RelatedConcepts)
            {
                dto.RelatedConcepts.Add(BusinessConceptRelationItem.ToDTO(currentItem));
            }

            return dto;
        }

    }
}
