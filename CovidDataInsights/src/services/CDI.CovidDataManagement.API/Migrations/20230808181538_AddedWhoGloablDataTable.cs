using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDI.CovidDataManagement.API.Migrations
{
    public partial class AddedWhoGloablDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhoGlobalData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntegrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateReported = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(65)", maxLength: 65, nullable: false),
                    WhoRegion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NewCases = table.Column<long>(type: "bigint", nullable: true),
                    CumulativeCases = table.Column<long>(type: "bigint", nullable: true),
                    NewDeaths = table.Column<long>(type: "bigint", nullable: true),
                    CumulativeDeaths = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoGlobalData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhoGlobalData_IntegrationData_IntegrationId",
                        column: x => x.IntegrationId,
                        principalTable: "IntegrationData",
                        principalColumn: "IntegrationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhoGlobalData_IntegrationId",
                table: "WhoGlobalData",
                column: "IntegrationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhoGlobalData");
        }
    }
}
