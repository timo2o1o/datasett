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

    }
}
