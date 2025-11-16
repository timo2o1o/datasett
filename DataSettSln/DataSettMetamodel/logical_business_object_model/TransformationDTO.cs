using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class TransformationDTO : TransformationBase
    {
        public string? SourceInterfaceId { get; set; }

        public string? SourceAttributeName { get; set; }
                
    }
}
