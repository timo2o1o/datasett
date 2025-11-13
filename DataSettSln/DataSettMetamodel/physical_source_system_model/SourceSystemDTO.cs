using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{

    public class SourceSystemDTO : SourceSystemBase
    {
        [JsonConstructor]
        public SourceSystemDTO()
        {
        }

        /// <summary>
        /// Unique identifier for the source system
        /// </summary>
        [JsonIgnore]
        public string? SourceSystemId {
            get
            {
                return this.Name;
            }
        }

    }

}