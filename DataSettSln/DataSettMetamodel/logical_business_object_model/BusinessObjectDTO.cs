using DataSett.Metamodel.logical_business_object_model;
using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    
    public class BusinessObjectDTO : BusinessObjectBase
    {
        public BusinessObjectDTO()
        {
            KeyParts = new List<BusinessObjectKeyPartDTO>();
        }

        [JsonIgnore]
        public string BusinessObjectId
        {
            get
            {
                return string.Format("{0}.{1}", BusinessDomainId, Name);
            }
        }

        public IList<BusinessObjectKeyPartDTO> KeyParts { get; set; }

        /// <summary>
        /// Reference to parent business domain
        /// </summary>
        public string? BusinessDomainId { get; set; }

    }
}
