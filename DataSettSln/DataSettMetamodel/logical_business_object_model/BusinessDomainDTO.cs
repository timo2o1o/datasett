using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for BusinessDomain.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID (not navigation properties).
    /// Navigation properties are excluded to allow serialization into separate JSON files.
    /// </summary>
    public class BusinessDomainDTO : BusinessDomainBase
    {
        [JsonConstructor]
        public BusinessDomainDTO()
        {
            BusinessObjectIds = new List<string>();
            BusinessRelations = new List<BusinessObjectRelation>();
        }

        public BusinessDomainDTO(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Reference to parent/hierarchy domain (nullable for root domains)
        /// </summary>
        [JsonPropertyName("hierarchy")]
        public BusinessDomainDTO? Hierarchy { get; set; }

        /// <summary>
        /// List of Business Object IDs (foreign key references)
        /// </summary>
        [JsonPropertyName("businessObjects")]
        public IList<string> BusinessObjectIds { get; set; }

        /// <summary>
        /// List of business relations within this domain
        /// </summary>
        [JsonPropertyName("businessRelations")]
        public IList<BusinessObjectRelation>? BusinessRelations { get; set; }
    }
}
