using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    public class BusinessObjectRelation : BusinessObjectRelationBase
    {

        public BusinessObjectRelation()
        {
            RelatedObjects = new List<BusinessObjectRelationItem>();
        }

        public IList<BusinessObjectRelationItem>? RelatedObjects { get; set; }

    }
}
