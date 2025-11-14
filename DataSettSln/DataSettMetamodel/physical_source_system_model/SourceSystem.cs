using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Represents a physical source system, like a database or a file system, from which data can be extracted.
    /// </summary>
    public class SourceSystem : SourceSystemBase
    {
        public SourceSystem()
        {
            SourceInterfaces = new List<SourceInterface>();
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to source interfaces within this system
        /// </summary>
        [JsonIgnore]
        public IList<SourceInterface> SourceInterfaces { get; set; }

        public static SourceSystem FromDTO(SourceSystemDTO current_sourceSystem_dto)
        {
            return new SourceSystem()
            {
                ConnectionString = current_sourceSystem_dto.ConnectionString,
                Driver = current_sourceSystem_dto.Driver,
                Name = current_sourceSystem_dto.Name,
                Server = current_sourceSystem_dto.Server,
                Version = current_sourceSystem_dto.Version
            };
        }
    }
}
