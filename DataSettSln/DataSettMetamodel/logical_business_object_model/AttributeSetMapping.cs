using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for AttributeSetMapping containing navigation properties and business logic.
    /// Inherits from AttributeSetMappingBase which contains scalar/context properties.
    /// This represents the connection between a physical source attribute and a business object.
    /// For serialization, use AttributeSetMappingDTO instead.
    /// </summary>
    public class AttributeSetMapping : AttributeSetMappingBase
    {
        [JsonConstructor]
        public AttributeSetMapping()
        {
        }

        // Navigation Properties
        /// <summary>
        /// Navigation property to source attribute
        /// </summary>
        [JsonIgnore]
        public SourceAttribute? SourceAttribute { get; set; }

        /// <summary>
        /// Navigation property to attribute set
        /// </summary>
        [JsonIgnore]
        public AttributeSet? AttributeSet { get; set; }

        /// <summary>
        /// Converts this attribute set mapping to a DTO for serialization
        /// </summary>
        public AttributeSetMappingDTO ToDTO()
        {
            return new AttributeSetMappingDTO
            {
                OrderNo = this.OrderNo,
                HistoryType = this.HistoryType,
                Role = this.Role,
                Position = this.Position,
                Default = this.Default,
                Nullable = this.Nullable,
                Datatype = this.Datatype,
                Length = this.Length,
                Precision = this.Precision,
                AttributeSetId = this.AttributeSet?.BusinessObject?.Name != null && this.AttributeSet?.Name != null 
                    ? $"{this.AttributeSet.BusinessObject.Name}.{this.AttributeSet.Name}" 
                    : null,
                SourceAttributeName = this.SourceAttribute?.Name
                // SourceInterfaceId would need to be obtained from SourceAttribute's parent
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static AttributeSetMapping FromDTO(AttributeSetMappingDTO dto)
        {
            return new AttributeSetMapping
            {
                OrderNo = dto.OrderNo,
                HistoryType = dto.HistoryType,
                Role = dto.Role,
                Position = dto.Position,
                Default = dto.Default,
                Nullable = dto.Nullable,
                Datatype = dto.Datatype,
                Length = dto.Length,
                Precision = dto.Precision
            };
        }
    }
}
