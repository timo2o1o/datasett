using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSett.Metamodel
{

    /// <summary>
    /// This describes mostly physical properties of an attribute.
    /// </summary>
    public class AttributeProperties
        // Could be a record?
    {

        /// <summary>
        /// Position of the attribute in the data structure
        /// </summary>
        public int? Position { get; set; }

        /// <summary>
        /// Default value
        /// </summary>
        public string? Default { get; set; }

        /// <summary>
        /// Whether the attribute is nullable
        /// </summary>
        public bool? Nullable { get; set; }

        /// <summary>
        /// Data type of the attribute
        /// </summary>
        public string? Datatype { get; set; }

        /// <summary>
        /// Length of the attribute
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Precision of the attribute
        /// </summary>
        public int? Precision { get; set; }


        public AttributeProperties Copy()
        {
            return new AttributeProperties
            {
                Position = this.Position,
                Default = this.Default,
                Nullable = this.Nullable,
                Datatype = this.Datatype,
                Length = this.Length,
                Precision = this.Precision
            };
        }

    }
}
