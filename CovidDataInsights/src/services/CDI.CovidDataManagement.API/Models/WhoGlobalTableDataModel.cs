using CDI.Core.DomainObjects;

namespace CDI.CovidDataManagement.API.Models
{
    public class WhoGlobalTableDataModel : Entity
    {
        public Guid IntegrationId { get; set; }
        public string? Name { get; set; }
        public string? WhoRegion { get; set; }
        public int CasesCumulativeTotal { get; set; }
        public double CasesCumulativeTotal_Per100000_Population { get; set; }
        public int CasesNewlyReportedInLast7Days { get; set; }
        public double CasesNewlyReportedInLast7Days_Per100000_Population { get; set; }
        public int CasesNewlyReportedInLast24Hours { get; set; }
        public int DeathsCumulativeTotal { get; set; }
        public double DeathsCumulativeTotal_Per100000_Population { get; set; }
        public int DeathsNewlyReportedInLast7Days { get; set; }
        public double DeathsNewlyReportedInLast7Days_Per100000_Population { get; set; }
        public int DeathsNewlyReportedInLast24Hours { get; set; }

        // Foreign key
        public IntegrationModel? IntegrationData { get; set; }
    }
}