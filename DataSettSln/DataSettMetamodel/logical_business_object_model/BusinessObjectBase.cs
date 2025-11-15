using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public abstract class BusinessObjectBase
    {
        /// <summary>
        /// Name of the business object
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
