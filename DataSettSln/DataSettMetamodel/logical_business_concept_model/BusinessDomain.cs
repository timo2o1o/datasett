using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Represents a category for business concepts.
    /// </summary>
    public class BusinessDomain : BusinessDomainBase
    {
        
        public BusinessDomain()
        {
            BusinessConcepts = new List<BusinessConcept>();
            ChildBusinessDomains = new List<BusinessDomain>();
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent/hierarchy domain
        /// </summary>
        [JsonIgnore]
        public BusinessDomain? ParentBusinessDomain { get; set; }

        public IList<BusinessDomain> ChildBusinessDomains { get; set; }

        /// <summary>
        /// Navigation property to business concepts in this domain
        /// </summary>
        [JsonIgnore]
        public IList<BusinessConcept> BusinessConcepts { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessDomain FromDTO(BusinessDomainDTO dto, BusinessDomain? parentBusinessDomain)
        {
            return new BusinessDomain
            {
                Name = dto.Name,
                ParentBusinessDomain = parentBusinessDomain
            };
        }
    }
}