using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// An attribute set is a grouping of attributes of a business object.
    /// A possible usage is satellite splitting in data vault models.
    /// </summary>
    public class AttributeSet
    {

        public AttributeSet(string name, BusinessObject businessObject)
        {
            BusinessObject = businessObject;
            Name = name;
            Id = string.Format("{0}.{1}", BusinessObject?.Id, Name);
            BusinessObjectId = BusinessObject?.Id;
        }

        // Identification Properties
        [JsonPropertyName("attributeSetId")]
        public string? Id { get; set; } = null;
        
        [JsonPropertyName("businessObjectId")]
        public string? BusinessObjectId { get; set; }

        // Context properties
        [JsonPropertyName("name")]
        public string? Name { get; set; } = null;

        // Navigation Properties
        [JsonIgnore]
        public BusinessObject? BusinessObject { get; set; }

    }
}
