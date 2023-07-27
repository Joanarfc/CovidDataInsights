using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDI.CovidDataManagement.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationData",
                columns: table => new
                {
                    IntegrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    NumberOfRows = table.Column<int>(type: "int", precision: 7, nullable: false),
                    RowsIntegrated = table.Column<int>(type: "int", precision: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationData", x => x.IntegrationId);
                });

            migrationBuilder.CreateTable(
                name: "VaccinationData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ISO3 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    WhoRegion = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalVaccinations = table.Column<long>(type: "bigint", nullable: true),
                    PersonsVaccinated_1Plus_Dose = table.Column<long>(type: "bigint", nullable: true),
                    TotalVaccinations_Per100 = table.Column<double>(type: "float", nullable: true),
                    PersonsVaccinated_1Plus_Dose_Per100 = table.Column<double>(type: "float", nullable: true),
                    PersonsLastDose = table.Column<long>(type: "bigint", nullable: true),
                    PersonsLastDosePer100 = table.Column<double>(type: "float", nullable: true),
                    VaccinesUsed = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    FirstVaccineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberVaccinesTypesUsed = table.Column<int>(type: "int", nullable: true),
                    PersonsBoosterAddDose = table.Column<long>(type: "bigint", nullable: true),
                    PersonsBoosterAddDose_Per100 = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccinationData_IntegrationData_IntegrationId",
                        column: x => x.IntegrationId,
                        principalTable: "IntegrationData",
                        principalColumn: "IntegrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationData_IntegrationId",
                table: "VaccinationData",
                column: "IntegrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VaccinationData");

            migrationBuilder.DropTable(
                name: "IntegrationData");
        }
    }
}
