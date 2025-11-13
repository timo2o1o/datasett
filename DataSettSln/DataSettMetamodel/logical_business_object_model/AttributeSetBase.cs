using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Base class for AttributeSet containing scalar and context properties.
    /// This class is part of the Base/DTO/Domain separation pattern.
    /// An attribute set is a grouping of attributes of a business object.
    /// A possible usage is satellite splitting in data vault models.
    /// </summary>
    public abstract class AttributeSetBase
    {
        /// <summary>
        /// Unique identifier for the attribute set
        /// </summary>
        [JsonPropertyName("attributeSetId")]
        public string? Id { get; set; }
        
        /// <summary>
        /// Name of the attribute set
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
