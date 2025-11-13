using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for BusinessObject containing navigation properties and business logic.
    /// Inherits from BusinessObjectBase which contains scalar/context properties.
    /// For serialization, use BusinessObjectDTO instead.
    /// </summary>
    public class BusinessObject : BusinessObjectBase
    {
        [JsonConstructor]
        public BusinessObject()
        {
            AttributeSets = new List<AttributeSet>();
            AttributeSetIds = new List<string>();
        }

        public BusinessObject(string name, BusinessDomain? businessDomain = null)
        {
            Name = name;
            Id = string.Format("{0}.{1}", businessDomain?.Name, Name);
            AttributeSets = new List<AttributeSet>();
            AttributeSetIds = new List<string>();
            BusinessDomain = businessDomain;
        }

        // Collections for backward compatibility with ViewModels (not serialized)
        /// <summary>
        /// List of Attribute Set IDs - maintained for backward compatibility
        /// </summary>
        [JsonIgnore]
        public IList<string> AttributeSetIds { get; set; }

        // Navigation Properties
        /// <summary>
        /// Navigation property to attribute sets
        /// </summary>
        [JsonIgnore]
        public IList<AttributeSet>? AttributeSets { get; set; }

        /// <summary>
        /// Navigation property to parent business domain
        /// </summary>
        [JsonIgnore]
        public BusinessDomain? BusinessDomain { get; set; }

        /// <summary>
        /// Converts this business object to a DTO for serialization
        /// </summary>
        public BusinessObjectDTO ToDTO()
        {
            return new BusinessObjectDTO
            {
                Id = this.Id,
                Name = this.Name,
                AttributeSetIds = this.AttributeSets?.Select(a => a.Id ?? "").Where(id => !string.IsNullOrEmpty(id)).ToList() ?? new List<string>()
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessObject FromDTO(BusinessObjectDTO dto)
        {
            return new BusinessObject
            {
                Id = dto.Id,
                Name = dto.Name,
                AttributeSets = new List<AttributeSet>(),
                AttributeSetIds = dto.AttributeSetIds?.ToList() ?? new List<string>()
            };
        }
    }
}
