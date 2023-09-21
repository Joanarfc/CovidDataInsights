namespace CDI.CovidApp.MVC.Models
{
    public class CovidWithGeoJsonDataViewModel
    {
        public CovidDataViewModel? CovidData { get; set; }
        public GeoJsonViewModel? GeoJsonData { get; set; }
    }
}