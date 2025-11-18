using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataSett.Metamodel.logical_business_object_model
{
    public class BusinessObjectKeyPartDTO : BusinessObjectKeyPartBase
    {

        [JsonIgnore]
        public string? BusinessObjectKeyPartId
        {
            get
            {
                return $"{BusinessObjectId}.{Name}";
            }
        }

        [JsonIgnore]
        public string? BusinessObjectId { get; set; }

    }
}
