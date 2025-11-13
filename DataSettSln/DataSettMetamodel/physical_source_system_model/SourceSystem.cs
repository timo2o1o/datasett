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
        public IList<SourceInterface>? SourceInterfaces { get; set; }

    }
}
