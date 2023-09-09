namespace CDI.CovidDataManagement.API.DTO
{
    public class CovidDataDto
    {
        public string? Region { get; set; }
        public long? TotalVaccineDoses { get; set; }
        public long? TotalPersonsVaccinatedAtLeastOneDose { get; set; }
        public long? TotalPersonsVaccinatedWithCompletePrimarySeries { get; set; }
    }
}