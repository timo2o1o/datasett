using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    
    public class BusinessDomainDTO : BusinessDomainBase
    {
        [JsonConstructor]
        public BusinessDomainDTO()
        {
            
        }

        [JsonIgnore]
        public string? BusinessDomainId
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Reference to parent/hierarchy domain (nullable for root domains)
        /// </summary>
        [JsonPropertyName("parentBusinessDomainId")]
        public string? ParentBusinessDomainId { get; set; }

    }
}
