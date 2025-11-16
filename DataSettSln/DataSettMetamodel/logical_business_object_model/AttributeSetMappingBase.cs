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
        public int? OrderNo { get; set; }

        /// <summary>
        /// History type for this attribute
        /// </summary>
        public HistoryType? HistoryType { get; set; }

        /// <summary>
        /// Role of the source attribute in business context
        /// </summary>
        public SourceAttributeRole? Role { get; set; }

        /// <summary>
        /// Attribute properties in the attribute set
        /// This might override the properties of the source attribute.
        /// </summary>
        public AttributeProperties? AttributeProperties { get; set; }
    }
}
