using CDI.Core.DomainObjects;

namespace CDI.CovidDataManagement.API.Models
{
    public class WhoGlobalDataModel : Entity
    {
        public Guid IntegrationId { get; set; }
        public string? DateReported { get; set; }
        public string? CountryCode { get; set; }
        public string? Country { get; set; }
        public string? WhoRegion { get; set; }
        public long? NewCases { get; set; }
        public long? CumulativeCases { get; set; }
        public long? NewDeaths { get; set; }
        public long? CumulativeDeaths { get; set; }

        // Foreign key
        public IntegrationModel? IntegrationData { get; set; }
    }
}