using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Base class for AttributeSetMapping containing scalar and context properties.
    /// This class is part of the Base/DTO/Domain separation pattern.
    /// This represents the connection between a physical source attribute and a business object.
    /// </summary>
    public abstract class AttributeSetMappingBase
    {
        /// <summary>
        /// Order number for the mapping
        /// </summary>
        [JsonPropertyName("orderNo")]
        public int? OrderNo { get; set; }

        /// <summary>
        /// History type for this attribute
        /// </summary>
        [JsonPropertyName("historyType")]
        public HistoryType? HistoryType { get; set; }

        /// <summary>
        /// Role of the source attribute in business context
        /// </summary>
        [JsonPropertyName("role")]
        public SourceAttributeRole? Role { get; set; }

        /// <summary>
        /// Position of the attribute
        /// </summary>
        [JsonPropertyName("position")]
        public int? Position { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        [JsonPropertyName("default")]
        public string? Default { get; set; }

        /// <summary>
        /// Whether the attribute is nullable
        /// </summary>
        [JsonPropertyName("nullable")]
        public bool? Nullable { get; set; }

        /// <summary>
        /// Data type of the attribute
        /// </summary>
        [JsonPropertyName("datatype")]
        public string? Datatype { get; set; }

        /// <summary>
        /// Length of the attribute
        /// </summary>
        [JsonPropertyName("length")]
        public int? Length { get; set; }

        /// <summary>
        /// Precision of the attribute
        /// </summary>
        [JsonPropertyName("precision")]
        public int? Precision { get; set; }
    }
}
