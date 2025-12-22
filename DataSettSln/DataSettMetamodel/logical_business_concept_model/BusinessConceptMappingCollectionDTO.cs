using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public class BusinessConceptMappingCollectionDTO
    {

        public BusinessConceptMappingCollectionDTO(string businessConceptId)
        {
            BusinessConceptId = businessConceptId;

            BusinessConceptMappings = new List<BusinessConceptMappingDTO>();
        }

        public string BusinessConceptId { get; set; }

        public List<BusinessConceptMappingDTO> BusinessConceptMappings { get; set; }

    }
}
