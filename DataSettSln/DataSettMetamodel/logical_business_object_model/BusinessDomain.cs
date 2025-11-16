using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Represents a category for business objects.
    /// </summary>
    public class BusinessDomain : BusinessDomainBase
    {
        
        public BusinessDomain()
        {
            BusinessObjects = new List<BusinessObject>();
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
        /// Navigation property to business objects in this domain
        /// </summary>
        [JsonIgnore]
        public IList<BusinessObject> BusinessObjects { get; set; }

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