using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    
    public class BusinessObjectDTO : BusinessObjectBase
    {
        public BusinessObjectDTO()
        {
            
        }

        [JsonIgnore]
        public string BusinessObjectId
        {
            get
            {
                return string.Format("{0}.{1}", BusinessDomainId, Name);
            }
        }

        /// <summary>
        /// Reference to parent business domain
        /// </summary>
        public string? BusinessDomainId { get; set; }

    }
}
