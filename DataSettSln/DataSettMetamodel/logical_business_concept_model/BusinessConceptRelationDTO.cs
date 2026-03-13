using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public class BusinessConceptRelationDTO : BusinessConceptRelationBase
    {

        public BusinessConceptRelationDTO()
        {
            RelatedConcepts = new List<BusinessConceptRelationItemDTO>();
        }

        [JsonIgnore]
        public string? BusinessConceptRelationId
        {
            get
            {
                return Name;
            }
        }

        public IList<BusinessConceptRelationItemDTO> RelatedConcepts { get; set; }

        /// <summary>
        /// Reference to parent business domain
        /// </summary>
        public string? BusinessDomainId { get; set; }

    }
}
