using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    public class BusinessObjectRelation
    {
        [JsonConstructor]
        public BusinessObjectRelation()
        {
            RelatedKeys = new List<BusinessObjectRelationItem>();
        }

        public BusinessObjectRelation(string name) : this()
        {
            Name = name;
        }

        [JsonPropertyName("businessRelationName")]
        public string? Name { get; set; }

        [JsonPropertyName("relatedKeys")]
        public IList<BusinessObjectRelationItem>? RelatedKeys { get; set; }
        
    }
}
