using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for SourceInterface.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// </summary>
    public class SourceInterfaceDTO : SourceInterfaceBase
    {
        [JsonConstructor]
        public SourceInterfaceDTO()
        {
            SourceAttributes = new List<SourceAttribute>();
        }

        /// <summary>
        /// List of source attributes (embedded for convenience in DTO)
        /// </summary>
        [JsonPropertyName("sourceAttributes")]
        public IList<SourceAttribute>? SourceAttributes { get; set; }
    }
}
