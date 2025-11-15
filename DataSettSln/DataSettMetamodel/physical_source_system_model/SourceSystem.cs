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
        public IList<SourceInterface> SourceInterfaces { get; set; }

        public static SourceSystem FromDTO(SourceSystemDTO dto)
        {
            return new SourceSystem()
            {
                ConnectionString = dto.ConnectionString,
                Driver = dto.Driver,
                Name = dto.Name,
                Server = dto.Server,
                Version = dto.Version
            };
        }
    }
}
