using CDI.Core.DomainObjects;

namespace CDI.CovidDataManagement.API.Models
{
    public class VaccinationMetaDataModel : Entity
    {
        public Guid IntegrationId { get; set; }
        public string? ISO3 { get; set; }
        public string? VaccineName { get; set; }
        public string? ProductName { get; set; }
        public string? CompanyName { get; set; }
        public string? AuthorizationDate { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Comment { get; set; }
        public string? DataSource { get; set; }

        // Foreign key
        public IntegrationModel? IntegrationData { get; set; }
    }
}