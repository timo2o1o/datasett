using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public class BusinessConceptKeyPartDTO : BusinessConceptKeyPartBase
    {

        [JsonIgnore]
        public string? BusinessConceptKeyPartId
        {
            get
            {
                return $"{BusinessConceptId}.{Name}";
            }
        }

        [JsonIgnore]
        public string? BusinessConceptId { get; set; }

    }
}
