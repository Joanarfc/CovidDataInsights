namespace CDI.CovidDataManagement.API.DTO
{
    public class CasesAndDeathsDataDto
    {
        public string? Region { get; set; }
        public int? NewCasesLast7Days { get; set; }
        public long? CumulativeCases { get; set; }
        public int? NewDeathsLast7Days { get; set; }
        public long? CumulativeDeaths { get; set; }
    }
}