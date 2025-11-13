using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for BusinessDomain containing navigation properties and business logic.
    /// Inherits from BusinessDomainBase which contains scalar/context properties.
    /// This class includes navigation properties that should not be serialized.
    /// For serialization, use BusinessDomainDTO instead.
    /// </summary>
    public class BusinessDomain : BusinessDomainBase
    {
        [JsonConstructor]
        public BusinessDomain()
        {
            BusinessObjects = new List<BusinessObject>();
            BusinessObjectIds = new List<string>();
            BusinessRelations = new List<BusinessObjectRelation>();
        }

        public BusinessDomain(string name) : this()
        {
            Name = name;
        }

        // Collections for backward compatibility with ViewModels (not serialized)
        /// <summary>
        /// List of Business Object IDs - maintained for backward compatibility
        /// </summary>
        [JsonIgnore]
        public IList<string> BusinessObjectIds { get; set; }

        /// <summary>
        /// List of business relations - maintained for backward compatibility
        /// </summary>
        [JsonIgnore]
        public IList<BusinessObjectRelation>? BusinessRelations { get; set; }

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent/hierarchy domain
        /// </summary>
        [JsonIgnore]
        public BusinessDomain? Hierarchy { get; set; }

        /// <summary>
        /// Navigation property to business objects in this domain
        /// </summary>
        [JsonIgnore]
        public IList<BusinessObject>? BusinessObjects { get; set; }

        /// <summary>
        /// Converts this domain entity to a DTO for serialization
        /// </summary>
        public BusinessDomainDTO ToDTO()
        {
            return new BusinessDomainDTO
            {
                Name = this.Name,
                Hierarchy = this.Hierarchy?.ToDTO(),
                BusinessObjectIds = this.BusinessObjects?.Select(bo => bo.Id ?? "").Where(id => !string.IsNullOrEmpty(id)).ToList() ?? new List<string>(),
                BusinessRelations = this.BusinessRelations ?? new List<BusinessObjectRelation>()
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessDomain FromDTO(BusinessDomainDTO dto)
        {
            return new BusinessDomain
            {
                Name = dto.Name,
                Hierarchy = dto.Hierarchy != null ? FromDTO(dto.Hierarchy) : null,
                BusinessObjects = new List<BusinessObject>(),
                BusinessObjectIds = dto.BusinessObjectIds?.ToList() ?? new List<string>(),
                BusinessRelations = dto.BusinessRelations ?? new List<BusinessObjectRelation>()
            };
        }
    }
}
