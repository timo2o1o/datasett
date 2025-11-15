using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for BusinessObject containing navigation properties and business logic.
    /// </summary>
    public class BusinessObject : BusinessObjectBase
    {
        [JsonConstructor]
        public BusinessObject()
        {
            AttributeSets = new List<AttributeSet>();
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to attribute sets
        /// </summary>
        public IList<AttributeSet> AttributeSets { get; set; }

        /// <summary>
        /// Navigation property to parent business domain
        /// </summary>
        [JsonIgnore]
        public BusinessDomain? ParentBusinessDomain { get; set; }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessObject FromDTO(BusinessObjectDTO dto, BusinessDomain parentBusinessDomain)
        {
            return new BusinessObject
            {
                Name = dto.Name,
                ParentBusinessDomain = parentBusinessDomain
            };
        }
    }
}
