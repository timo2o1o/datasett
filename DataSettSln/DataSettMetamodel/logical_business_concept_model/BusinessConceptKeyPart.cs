using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public class BusinessConceptKeyPart
    {

        public BusinessConceptKeyPart()
        {
            KeyProperties = new AttributeProperties();
        }

        public string? Name { get; set; }

        public AttributeProperties KeyProperties { get; set; }

        [JsonIgnore]
        public string? BusinessConceptKeyPartId
        {
            get
            {
                string bd = ParentBusinessConcept?.ParentBusinessDomain?.Name ?? "<no-domain>";
                string bc = ParentBusinessConcept?.Name ?? "<no-concept>";

                return $"{bd}.{bc}.{Name}";
            }
        }

        [JsonIgnore]
        public BusinessConcept? ParentBusinessConcept { get; set; }

    }
}
