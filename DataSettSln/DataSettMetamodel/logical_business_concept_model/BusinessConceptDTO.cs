using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    
    public class BusinessConceptDTO : BusinessConceptBase
    {

        [JsonIgnore]
        public string BusinessConceptId
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
