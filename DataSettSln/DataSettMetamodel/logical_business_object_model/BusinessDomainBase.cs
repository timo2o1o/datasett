using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
        
    public abstract class BusinessDomainBase
    {
        /// <summary>
        /// Name of the business domain
        /// </summary>
        public string? Name { get; set; }
    }
}
