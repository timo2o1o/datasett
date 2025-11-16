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

        public string? Name { get; set; }

        public IList<BusinessObjectRelationItem>? RelatedObjects { get; set; }
        
    }
}
