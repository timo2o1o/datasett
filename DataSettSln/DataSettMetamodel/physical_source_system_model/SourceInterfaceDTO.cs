using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{

    public class SourceInterfaceDTO : SourceInterfaceBase
    {
        [JsonConstructor]
        public SourceInterfaceDTO()
        {
        
        }

        /// <summary>
        /// Unique identifier for the source interface
        /// </summary>
        [JsonIgnore]
        public string? SourceInterfaceId
        {
            get
            {
                return $"{this.SourceSystemId}.{this.Name}";
            }
        }

        /// <summary>
        /// Unique identifier for the associated source system
        /// </summary>
        public string? SourceSystemId { get; set; }

    }

}