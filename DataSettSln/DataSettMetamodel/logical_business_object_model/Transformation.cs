using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This is used to define transformations of source attributes,
    /// like key concatenation, data type conversion, etc.
    /// </summary>
    public class Transformation : TransformationBase
    {
        public SourceAttribute? SourceAttribute { get; set; }           
    }
}
