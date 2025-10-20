using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class BusinessDomain
    {
        [JsonConstructor]
        public BusinessDomain()
        {
            BusinessObjects = new List<BusinessObject>();
            BusinessObjectIds = new List<string>();

            BusinessRelations = new List<BusinessObjectRelation>();
        }

        public BusinessDomain(string name) : this()
        {
            Name = name;
        }

        [JsonPropertyName("hierarchy")]
        public BusinessDomain? Hierarchy { get; set; }

        [JsonPropertyName("businessDomainName")]
        public string? Name { get; set; }

        [JsonPropertyName("businessObjects")]
        public IList<string> BusinessObjectIds { get; set; }

        [JsonPropertyName("businessRelations")]
        public IList<BusinessObjectRelation>? BusinessRelations { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public IList<BusinessObject>? BusinessObjects { get; set; }

    }
}
