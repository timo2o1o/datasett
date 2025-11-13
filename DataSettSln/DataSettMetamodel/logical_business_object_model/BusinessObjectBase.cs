using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Base class for BusinessObject containing scalar and context properties.
    /// This class is part of the Base/DTO/Domain separation pattern.
    /// </summary>
    public abstract class BusinessObjectBase
    {
        /// <summary>
        /// Unique identifier for the business object
        /// </summary>
        [JsonPropertyName("businessObjectId")]
        public string? Id { get; set; }

        /// <summary>
        /// Name of the business object
        /// </summary>
        [JsonPropertyName("businessObjectName")]
        public string? Name { get; set; }
    }
}
