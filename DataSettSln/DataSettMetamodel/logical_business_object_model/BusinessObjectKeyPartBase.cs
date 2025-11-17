using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{
    public class BusinessObjectKeyPartBase
    {

        public BusinessObjectKeyPartBase()
        {
            KeyProperties = new AttributeProperties();
        }

        public string? Name { get; set; }

        public AttributeProperties KeyProperties { get; set; }

    }
}
