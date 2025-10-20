using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    public class BusinessObject
    {
        [JsonConstructor]
        public BusinessObject()
        {
            AttributeSets = new List<AttributeSet>();
            AttributeSetIds = new List<string>();
        }

        public BusinessObject(string name, BusinessDomain? businessDomain = null)
        {
            Name = name;
            Id = string.Format("{0}.{1}", businessDomain?.Name, Name);

            AttributeSets = new List<AttributeSet>();
            AttributeSetIds = new List<string>();
        }

        [JsonPropertyName("businessObjectId")]
        public string? Id { get; set; }

        [JsonPropertyName("businessObjectName")]
        public string? Name { get; set; }

        [JsonPropertyName("attributeSets")]
        public IList<string> AttributeSetIds { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public IList<AttributeSet>? AttributeSets { get; set; }

        [JsonIgnore]
        public BusinessDomain? BusinessDomain { get; set; }

    }
}
