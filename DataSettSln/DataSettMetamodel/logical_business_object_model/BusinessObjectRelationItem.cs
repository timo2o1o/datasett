using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Describes leading key of a relation between business objects
    /// </summary>
    public class BusinessObjectRelationItem
    {
        /// <summary>
        /// Parent identifier
        /// </summary>
        public BusinessObject? RelatedBusinessObject { get; set; }

        /// <summary>
        /// Indicates if this is a leading key
        /// </summary>
        public bool? IsLeadingKey { get; set; }
    }
}
