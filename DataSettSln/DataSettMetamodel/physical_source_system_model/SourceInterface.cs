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

    }
}
