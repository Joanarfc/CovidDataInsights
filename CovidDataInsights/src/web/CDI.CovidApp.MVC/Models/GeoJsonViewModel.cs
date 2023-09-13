using System.Text.Json;
using System.Text.Json.Serialization;

namespace CDI.CovidApp.MVC.Models
{
    public class GeoJsonViewModel
    {
        [JsonPropertyName("featurecla")]
        public string? Featurecla { get; set; }
        [JsonPropertyName("sovereignt")]
        public string? Sovereignt { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("admin")]
        public string? Admin { get; set; }
        [JsonPropertyName("nameLong")]
        public string? NameLong { get; set; }
        [JsonPropertyName("formalEN")]
        public string? FormalEN { get; set; }
        [JsonPropertyName("nameEN")]
        public string? NameEN { get; set; }
        [JsonPropertyName("coordinates")]
        public string? Coordinates { get; set; }
    }
}