namespace CDI.CovidDataManagement.API.DTO
{
    public class VaccinationDataDto
    {
        public string? Region { get; set; }
        public long? TotalVaccineDoses { get; set; }
        public long? PersonsVaccinatedAtLeastOneDose { get; set; }
        public long? PersonsVaccinatedWithCompletePrimarySeries { get; set; }
    }
}