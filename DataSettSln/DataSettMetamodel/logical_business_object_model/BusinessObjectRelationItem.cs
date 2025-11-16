using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// Describes leading key of a relation between business objects
    /// </summary>
    public class BusinessObjectRelationItem : BusinessObjectRelationItemBase
    {
        /// <summary>
        /// Parent identifier
        /// </summary>
        public BusinessObject? RelatedBusinessObject { get; set; }

    }
}
