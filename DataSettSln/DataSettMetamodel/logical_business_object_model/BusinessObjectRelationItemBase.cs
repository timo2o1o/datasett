using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public abstract class BusinessObjectRelationItemBase
    {

        /// <summary>
        /// Indicates if this is a leading key
        /// </summary>
        public bool? IsLeadingKey { get; set; }

    }
}
