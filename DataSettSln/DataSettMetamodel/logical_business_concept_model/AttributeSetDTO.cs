using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for AttributeSet.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// </summary>
    public class AttributeSetDTO : AttributeSetBase
    {
        public AttributeSetDTO()
        {
            AttributeSetMappings = new List<AttributeSetMappingDTO>();
        }

        [JsonIgnore]
        public string? AttributeSetId
        {
            get
            {
                return $"{BusinessConceptId}.{Name}";
            }
        }

        /// <summary>
        /// Foreign key reference to parent business concept
        /// </summary>
        public string? BusinessConceptId { get; set; }

        public IList<AttributeSetMappingDTO> AttributeSetMappings { get; set; }
        }
}
