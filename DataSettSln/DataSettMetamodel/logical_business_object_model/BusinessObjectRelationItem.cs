using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Domain class for BusinessObjectRelationItem containing navigation properties and business logic.
    /// Inherits from BusinessObjectRelationItemBase which contains scalar/context properties.
    /// For serialization, use BusinessObjectRelationItemDTO instead.
    /// </summary>
    public class BusinessObjectRelationItem : BusinessObjectRelationItemBase
    {
        [JsonConstructor]
        public BusinessObjectRelationItem()
        {
        }

        // Property for backward compatibility with ViewModels (not serialized)
        /// <summary>
        /// Related key ID - maintained for backward compatibility
        /// </summary>
        [JsonIgnore]
        public string? RelatedKeyId { get; set; }

        // Navigation Properties
        /// <summary>
        /// Navigation property to the related business object
        /// </summary>
        [JsonIgnore]
        public BusinessObject? RelatedKey { get; set; }

        /// <summary>
        /// Converts this relation item to a DTO for serialization
        /// </summary>
        public BusinessObjectRelationItemDTO ToDTO()
        {
            return new BusinessObjectRelationItemDTO
            {
                Parent = this.Parent,
                IsLeadingKey = this.IsLeadingKey,
                RelatedKeyId = this.RelatedKey?.Id ?? this.RelatedKeyId
            };
        }

        /// <summary>
        /// Creates a domain entity from a DTO (navigation properties must be set separately)
        /// </summary>
        public static BusinessObjectRelationItem FromDTO(BusinessObjectRelationItemDTO dto)
        {
            return new BusinessObjectRelationItem
            {
                Parent = dto.Parent,
                IsLeadingKey = dto.IsLeadingKey,
                RelatedKeyId = dto.RelatedKeyId
            };
        }
    }
}
