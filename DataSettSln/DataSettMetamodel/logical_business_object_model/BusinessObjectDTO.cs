using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for BusinessObject.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// </summary>
    public class BusinessObjectDTO : BusinessObjectBase
    {
        [JsonConstructor]
        public BusinessObjectDTO()
        {
            AttributeSetIds = new List<string>();
        }

        public BusinessObjectDTO(string name, string? businessDomainName = null)
        {
            Name = name;
            if (!string.IsNullOrEmpty(businessDomainName))
            {
                Id = string.Format("{0}.{1}", businessDomainName, Name);
            }
            AttributeSetIds = new List<string>();
        }

        /// <summary>
        /// List of Attribute Set IDs (foreign key references)
        /// </summary>
        [JsonPropertyName("attributeSets")]
        public IList<string> AttributeSetIds { get; set; }
    }
}
