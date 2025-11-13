using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for SourceSystem containing navigation properties and business logic.
    /// Inherits from SourceSystemBase which contains scalar/context properties.
    /// Represents a physical source system, like a database or a file system, from which data can be extracted.
    /// For serialization, use SourceSystemDTO instead.
    /// </summary>
    public class SourceSystem : SourceSystemBase
    {
        [JsonConstructor]
        public SourceSystem()
        {
            SourceInterfaces = new List<SourceInterface>();
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to source interfaces within this system
        /// </summary>
        [JsonIgnore]
        public IList<SourceInterface>? SourceInterfaces { get; set; }

        /// <summary>
        /// Converts this source system to a DTO for serialization
        /// </summary>
        public SourceSystemDTO ToDTO()
        {
            return new SourceSystemDTO
            {
                SourceSystemId = this.SourceSystemId,
                Driver = this.Driver,
                ConnectionString = this.ConnectionString,
                Server = this.Server,
                Name = this.Name,
                Version = this.Version
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static SourceSystem FromDTO(SourceSystemDTO dto)
        {
            return new SourceSystem
            {
                SourceSystemId = dto.SourceSystemId,
                Driver = dto.Driver,
                ConnectionString = dto.ConnectionString,
                Server = dto.Server,
                Name = dto.Name,
                Version = dto.Version,
                SourceInterfaces = new List<SourceInterface>()
            };
        }
    }
}
