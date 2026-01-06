using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public class BusinessConceptKeyPart : BusinessConceptKeyPartBase
    {

        public BusinessConcept? ParentBusinessConcept { get; set; }

        internal static BusinessConceptKeyPart FromDTO(BusinessConceptKeyPartDTO dto, BusinessConcept parentConcept)
        {
            return new BusinessConceptKeyPart
            {
                Name = dto.Name,
                KeyProperties = dto.KeyProperties,
                ParentBusinessConcept = parentConcept
            };
        }

        internal static BusinessConceptKeyPartDTO ToDTO(BusinessConceptKeyPart bckp, string businessConceptId)
        {
            return new BusinessConceptKeyPartDTO
            {
                Name = bckp.Name,
                KeyProperties = bckp.KeyProperties,
                BusinessConceptId = businessConceptId
            };
        }
    }
}
