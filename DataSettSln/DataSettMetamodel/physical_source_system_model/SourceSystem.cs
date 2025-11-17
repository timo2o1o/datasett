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

            // We could think about using attributes to map these properties:
            return new SourceSystem()
            {
                ConnectionString = dto.ConnectionString,
                Driver = dto.Driver,
                Name = dto.Name,
                Server = dto.Server,
                ShortName = dto.ShortName,
                Version = dto.Version
            };
        }
    }
}
