using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    /// <summary>
    /// This is the definition of an attribute within a source interface.
    /// Could be a column of a table or a field in a file.
    /// </summary>
    public class SourceAttribute
    {
        
        // Context Properties
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("isPk")]
        public bool? IsPk { get; set; }

        [JsonPropertyName("isFk")]
        public bool? IsFk { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("default")]
        public string? Default { get; set; }

        [JsonPropertyName("nullable")]
        public bool? Nullable { get; set; }

        [JsonPropertyName("datatype")]
        public string? Datatype { get; set; }

        [JsonPropertyName("length")]
        public int? Length { get; set; }

        [JsonPropertyName("precision")]
        public int? Precision { get; set; }

    }
}