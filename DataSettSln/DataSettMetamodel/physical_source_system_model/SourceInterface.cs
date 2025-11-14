using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Source Interface represents a specific interface (like a table, file, API endpoint) 
    /// within a Source System from which data can be extracted.
    /// </summary>
    public class SourceInterface : SourceInterfaceBase
    {

        // Navigation Properties
        /// <summary>
        /// Navigation property to parent source system
        /// </summary>
        [JsonIgnore]
        public SourceSystem? ParentSourceSystem { get; set; }

        public static SourceInterface FromDTO(SourceInterfaceDTO current_sourceInterface_dto, SourceSystem newSourceSystem)
        {
            return new SourceInterface()
            {
                Catalog = current_sourceInterface_dto.Catalog,
                Name = current_sourceInterface_dto.Name,
                ParentSourceSystem = newSourceSystem,
                Schema = current_sourceInterface_dto.Schema,
                SourceAttributes = current_sourceInterface_dto.SourceAttributes
            };
        }
    }
}
