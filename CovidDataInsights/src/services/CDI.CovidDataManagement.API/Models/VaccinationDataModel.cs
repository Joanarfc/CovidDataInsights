using CDI.Core.DomainObjects;

namespace CDI.CovidDataManagement.API.Models
{
    public class VaccinationDataModel : Entity
    {
        public Guid IntegrationId { get; set; }
        public string? Country { get; set; }
        public string? ISO3 { get; set; }
        public string? WhoRegion { get; set; }
        public string? DataSource { get; set; }
        public DateTime DateUpdated { get; set; }
        public long? TotalVaccinations { get; set; }
        public long? PersonsVaccinated_1Plus_Dose { get; set; }
        public double? TotalVaccinations_Per100 { get; set; }
        public double? PersonsVaccinated_1Plus_Dose_Per100 { get; set; }
        public long? PersonsLastDose { get; set; }
        public double? PersonsLastDosePer100 { get; set; }
        public string? VaccinesUsed { get; set; }
        public DateTime? FirstVaccineDate { get; set; }
        public int? NumberVaccinesTypesUsed { get; set; }
        public long? PersonsBoosterAddDose { get; set; }
        public double? PersonsBoosterAddDose_Per100 { get; set; }

        // Foreign key
        public IntegrationModel? IntegrationData { get; set; }
    }
}