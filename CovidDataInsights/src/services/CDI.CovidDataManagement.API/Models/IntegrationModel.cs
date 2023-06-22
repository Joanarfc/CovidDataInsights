namespace CDI.CovidDataManagement.API.Models
{
    public class IntegrationModel
    {
        public DateTime IntegrationTimestamp { get; set; }
        public string? FileName { get; set; }
        public int NumberOfRows { get; set; }
        public int RowsIntegrated { get; set; }
    }
}