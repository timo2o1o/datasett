using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// DTO (Data Transfer Object) class for SourceSystem.
    /// Contains only serializable properties and foreign key references.
    /// Used for JSON serialization, supporting references by ID.
    /// Navigation properties are excluded to allow serialization into separate JSON files.
    /// </summary>
    public class SourceSystemDTO : SourceSystemBase
    {
        [JsonConstructor]
        public SourceSystemDTO()
        {
        }
    }
}
