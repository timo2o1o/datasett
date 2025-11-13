using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for SourceInterface containing navigation properties and business logic.
    /// Inherits from SourceInterfaceBase which contains scalar/context properties.
    /// Source Interface represents a specific interface (like a table, file, API endpoint) 
    /// within a Source System from which data can be extracted.
    /// For serialization, use SourceInterfaceDTO instead.
    /// </summary>
    public class SourceInterface : SourceInterfaceBase
    {
        [JsonConstructor]
        public SourceInterface()
        {
            SourceAttributes = new List<SourceAttribute>();
        }

        /// <summary>
        /// List of source attributes (for convenience, can be serialized)
        /// </summary>
        [JsonIgnore]
        public IList<SourceAttribute>? SourceAttributes { get; set; }

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent source system
        /// </summary>
        [JsonIgnore]
        public SourceSystem? ParentSourceSystem { get; set; }

        /// <summary>
        /// Converts this source interface to a DTO for serialization
        /// </summary>
        public SourceInterfaceDTO ToDTO()
        {
            return new SourceInterfaceDTO
            {
                SourceInterfaceId = this.SourceInterfaceId,
                Schema = this.Schema,
                Catalog = this.Catalog,
                Name = this.Name,
                SourceAttributes = this.SourceAttributes
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static SourceInterface FromDTO(SourceInterfaceDTO dto)
        {
            return new SourceInterface
            {
                SourceInterfaceId = dto.SourceInterfaceId,
                Schema = dto.Schema,
                Catalog = dto.Catalog,
                Name = dto.Name,
                SourceAttributes = dto.SourceAttributes ?? new List<SourceAttribute>()
            };
        }
    }
}
