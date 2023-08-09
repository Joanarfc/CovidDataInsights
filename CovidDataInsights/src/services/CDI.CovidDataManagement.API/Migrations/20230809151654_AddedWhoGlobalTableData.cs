using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDI.CovidDataManagement.API.Migrations
{
    public partial class AddedWhoGlobalTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhoGlobalTableData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    WhoRegion = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CasesCumulativeTotal = table.Column<int>(type: "int", nullable: false),
                    CasesCumulativeTotal_Per100000_Population = table.Column<double>(type: "float", nullable: false),
                    CasesNewlyReportedInLast7Days = table.Column<int>(type: "int", nullable: false),
                    CasesNewlyReportedInLast7Days_Per100000_Population = table.Column<double>(type: "float", nullable: false),
                    CasesNewlyReportedInLast24Hours = table.Column<int>(type: "int", nullable: false),
                    DeathsCumulativeTotal = table.Column<int>(type: "int", nullable: false),
                    DeathsCumulativeTotal_Per100000_Population = table.Column<double>(type: "float", nullable: false),
                    DeathsNewlyReportedInLast7Days = table.Column<int>(type: "int", nullable: false),
                    DeathsNewlyReportedInLast7Days_Per100000_Population = table.Column<double>(type: "float", nullable: false),
                    DeathsNewlyReportedInLast24Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoGlobalTableData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhoGlobalTableData_IntegrationData_IntegrationId",
                        column: x => x.IntegrationId,
                        principalTable: "IntegrationData",
                        principalColumn: "IntegrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhoGlobalTableData_IntegrationId",
                table: "WhoGlobalTableData",
                column: "IntegrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhoGlobalTableData");
        }
    }
}
