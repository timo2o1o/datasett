using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    internal class BusinessObjectRelationDTO : BusinessObjectRelationBase
    {

        public BusinessObjectRelationDTO()
        {
            RelatedObjects = new List<BusinessObjectRelationItemDTO>();
        }

        [JsonIgnore]
        public string? BusinessObjectRelationId
        {
            get
            {
                return Name;
            }
        }

        public IList<BusinessObjectRelationItemDTO> RelatedObjects { get; set; }

    }
}
