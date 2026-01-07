using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for BusinessConcept containing navigation properties and business logic.
    /// </summary>
    public class BusinessConcept : BusinessConceptBase
    {
        public BusinessConcept()
        {
            BusinessConceptMappings = new List<BusinessConceptMapping>();
        }
                
        /// <summary>
        /// Navigation property to parent business domain
        /// </summary>
        [JsonIgnore]
        public BusinessDomain? ParentBusinessDomain { get; set; }

        public IList<BusinessConceptMapping> BusinessConceptMappings { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessConcept FromDTO(BusinessConceptDTO dto, BusinessDomain parentBusinessDomain)
        {
            BusinessConcept newConcept = new BusinessConcept
            {
                Name = dto.Name,
                KeyParts = dto.KeyParts,
                ParentBusinessDomain = parentBusinessDomain
            };

            foreach (BusinessConceptKeyPart currentKP in newConcept.KeyParts)
            {
                currentKP.ParentBusinessConcept = newConcept;
            }

            return newConcept;
        }

        public static BusinessConceptDTO ToDTO(BusinessConcept concept, string businessDomainID)
        {
            BusinessConceptDTO newConcept = new BusinessConceptDTO
            {
                Name = concept.Name,
                KeyParts = concept.KeyParts,
                BusinessDomainId = businessDomainID
            };

            return newConcept;
        }

        public override string ToString()
        {
            var parentName = ParentBusinessDomain?.Name ?? "<no-domain>";
            var conceptName = Name ?? "<no-name>";
            return $"{parentName}.{conceptName}";
        }
    }
}
