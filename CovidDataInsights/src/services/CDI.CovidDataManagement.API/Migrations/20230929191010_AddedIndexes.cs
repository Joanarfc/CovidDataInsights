using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDI.CovidDataManagement.API.Migrations
{
    public partial class AddedIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IntegrationId",
                table: "WhoGlobalTableData",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationId",
                table: "VaccinationData",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_CountryName",
                table: "WhoGlobalTableData",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Country",
                table: "VaccinationData",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_FileName",
                table: "IntegrationData",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationId",
                table: "IntegrationData",
                column: "IntegrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CountryName",
                table: "WhoGlobalTableData");

            migrationBuilder.DropIndex(
                name: "IX_Country",
                table: "VaccinationData");

            migrationBuilder.DropIndex(
                name: "IX_FileName",
                table: "IntegrationData");

            migrationBuilder.DropIndex(
                name: "IX_IntegrationId",
                table: "IntegrationData");

            migrationBuilder.DropIndex(
                name: "IX_IntegrationId",
                table: "WhoGlobalTableData");

            migrationBuilder.DropIndex(
                name: "IX_IntegrationId",
                table: "VaccinationData");
        }
    }
}
