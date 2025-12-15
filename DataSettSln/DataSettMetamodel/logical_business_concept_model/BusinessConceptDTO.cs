using DataSett.Metamodel.logical_business_object_model;
using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    
    public class BusinessConceptDTO : BusinessConceptBase
    {
        public BusinessConceptDTO()
        {
            KeyParts = new List<BusinessConceptKeyPartDTO>();
        }

        [JsonIgnore]
        public string BusinessConceptId
        {
            get
            {
                return string.Format("{0}.{1}", BusinessDomainId, Name);
            }
        }

        public IList<BusinessConceptKeyPartDTO> KeyParts { get; set; }

        /// <summary>
        /// Reference to parent business domain
        /// </summary>
        public string? BusinessDomainId { get; set; }

    }
}
