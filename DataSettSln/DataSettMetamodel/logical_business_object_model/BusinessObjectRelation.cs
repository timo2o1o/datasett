using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    public class BusinessObjectRelation
    {
        [JsonConstructor]
        public BusinessObjectRelation()
        {
            RelatedObjects = new List<BusinessObjectRelationItem>();
        }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("relatedKeys")]
        public IList<BusinessObjectRelationItem>? RelatedObjects { get; set; }
        
    }
}
