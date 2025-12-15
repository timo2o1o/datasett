using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace DataSett.Metamodel
{
    public class BusinessConceptRelation : BusinessConceptRelationBase
    {

        public BusinessConceptRelation()
        {
            RelatedConcepts = new List<BusinessConceptRelationItem>();
        }

        public IList<BusinessConceptRelationItem>? RelatedConcepts { get; set; }

    }
}
