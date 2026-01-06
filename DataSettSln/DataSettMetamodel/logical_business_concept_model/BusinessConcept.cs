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
            KeyParts = new List<BusinessConceptKeyPart>();
            BusinessConceptMappings = new List<BusinessConceptMapping>();
        }

        /// <summary>
        /// Each Business Concept contains of one or more Key Parts
        /// </summary>
        public IList<BusinessConceptKeyPart> KeyParts { get; set; }
                
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
            return new BusinessConcept
            {
                Name = dto.Name,
                ParentBusinessDomain = parentBusinessDomain
            };
        }

        public override string ToString()
        {
            var parentName = ParentBusinessDomain?.Name ?? "<no-domain>";
            var conceptName = Name ?? "<no-name>";
            return $"{parentName}.{conceptName}";
        }
    }
}
