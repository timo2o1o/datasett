using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public abstract class BusinessConceptBase
    {

        public BusinessConceptBase()
        {
            KeyParts = new List<BusinessConceptKeyPart>();
        }

        /// <summary>
        /// Each Business Concept contains of one or more Key Parts
        /// </summary>
        public IList<BusinessConceptKeyPart> KeyParts { get; set; }

        /// <summary>
        /// Name of the business concept
        /// </summary>
        public string? Name { get; set; }
    }
}
