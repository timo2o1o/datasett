using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class SourceAttributeRelation
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; } = "";

        [JsonPropertyName("relationType")]
        public SourceAttributeRelationType? RelationType { get; set; }

        [JsonPropertyName("order")]
        public int? Order { get; set; }

        [JsonPropertyName("localKey")]
        public string? LocalKey { get; set; } = "";

        [JsonPropertyName("parentTable")]
        public string? ParentTable { get; set; } = "";

        [JsonPropertyName("parentKey")]
        public string? ParentKey { get; set; } = "";

        public SourceAttributeRelation()
        {
            RelationType = SourceAttributeRelationType.Undefined;
        }
    }
}
