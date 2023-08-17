using CDI.Core.DomainObjects;

namespace CDI.GeoSpatialDataLoader.API.Models
{
    public class GeoSpatialModel : Entity
    {
        public string? Featurecla { get; set; }
        public string? Sovereignt { get; set; }
        public string? Type { get; set; }
        public string? Admin { get; set; }
        public string? NameLong { get; set; }
        public string? FormalEN { get; set; }
        public string? NameEN { get; set; }
        public string? Coordinates { get; set; }
    }
}