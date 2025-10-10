using System.Text.Json.Serialization;

namespace DataSett.Metamodel
{
    public class HistoryType
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; } = null;
    }
}
