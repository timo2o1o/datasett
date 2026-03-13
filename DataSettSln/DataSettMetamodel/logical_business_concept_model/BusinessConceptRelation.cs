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

    }
}
