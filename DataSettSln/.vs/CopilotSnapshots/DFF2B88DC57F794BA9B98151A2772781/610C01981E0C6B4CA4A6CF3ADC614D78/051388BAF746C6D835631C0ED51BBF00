using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class AttributeSet
    {
        [JsonConstructor]
        public AttributeSet()
        {

        }

        public AttributeSet(string name, BusinessObject businessObject)
        {
            BusinessObject = businessObject;
            Name = name;
            Id = string.Format("{0}.{1}", BusinessObject?.Id, Name);
            BusinessObjectId = BusinessObject?.Id;
        }

        [JsonPropertyName("attributeSetId")]
        public string? Id { get; set; } = null;

        [JsonPropertyName("name")]
        public string? Name { get; set; } = null;

        [JsonIgnore]
        public BusinessObject? BusinessObject { get; set; }

        [JsonPropertyName("businessObjectId")]
        public string? BusinessObjectId { get; set; }
    }
}
