using CDI.CovidDataManagement.API.Models;

namespace CDI.CovidDataManagement.API.Factories
{
    public static class IntegrationRecordFactory
    {
        public static IntegrationModel CreateIntegrationRecord(string fileName, int numberOfRows, int rowsIntegrated)
        {
            return new IntegrationModel
            {
                IntegrationTimestamp = DateTime.UtcNow,
                FileName = fileName,
                NumberOfRows = numberOfRows,
                RowsIntegrated = rowsIntegrated
            };
        }
    }
}