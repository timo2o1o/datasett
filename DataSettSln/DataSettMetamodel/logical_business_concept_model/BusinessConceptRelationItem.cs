using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Describes leading key of a relation between business concepts
    /// </summary>
    public class BusinessConceptRelationItem : BusinessConceptRelationItemBase
    {
        /// <summary>
        /// Parent identifier
        /// </summary>
        public BusinessConcept? RelatedBusinessConcept { get; set; }

    }
}
