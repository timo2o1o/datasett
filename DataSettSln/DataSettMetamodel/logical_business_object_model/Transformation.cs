using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This is used to define transformations of source attributes,
    /// like key concatenation, data type conversion, etc.
    /// </summary>
    public class Transformation
    {
        public string? SourceInterfaceId { get; set; }

        public string? SourceAttributeName { get; set; }

        public string? TransformationExpression { get; set; }
    }
}
