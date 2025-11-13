using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Base class for BusinessObjectRelationItem containing scalar and context properties.
    /// This class is part of the Base/DTO/Domain separation pattern.
    /// </summary>
    public abstract class BusinessObjectRelationItemBase
    {
        /// <summary>
        /// Parent identifier
        /// </summary>
        [JsonPropertyName("parent")]
        public string? Parent { get; set; }

        /// <summary>
        /// Indicates if this is a leading key
        /// </summary>
        [JsonPropertyName("isLeadingKey")]
        public bool? IsLeadingKey { get; set; }
    }
}
