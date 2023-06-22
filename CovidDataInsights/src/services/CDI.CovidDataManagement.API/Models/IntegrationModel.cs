using CDI.Core.DomainObjects;

namespace CDI.CovidDataManagement.API.Models
{
    public class IntegrationModel : Entity
    {
        public DateTime IntegrationTimestamp { get; set; }
        public string? FileName { get; set; }
        public int NumberOfRows { get; set; }
        public int RowsIntegrated { get; set; }
    }
}