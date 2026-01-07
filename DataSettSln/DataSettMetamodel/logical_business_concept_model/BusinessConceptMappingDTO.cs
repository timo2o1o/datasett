using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for BusinessConceptMapping.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// </summary>
    public class BusinessConceptMappingDTO : BusinessConceptMappingBase
    {
       
        /// <summary>
        /// Foreign key reference to source interface
        /// </summary>
        public string? SourceInterfaceId { get; set; }

        /// <summary>
        /// Foreign key reference to source attribute
        /// </summary>
        public string? SourceAttributeName { get; set; }

        /// <summary>
        /// Foreign key reference to parent business concept
        /// </summary>
        [JsonIgnore]
        public string? BusinessConceptId { get; set; }

        public string? BusinessConceptKeyPartName { get; set; }

    }
}
