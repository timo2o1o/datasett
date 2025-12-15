using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public abstract class BusinessConceptBase
    {
        /// <summary>
        /// Name of the business concept
        /// </summary>
        public string? Name { get; set; }
    }
}
