using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDI.GeoSpatialDataLoader.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    featurecla = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    sovereignt = table.Column<string>(type: "character varying(35)", maxLength: 35, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    admin = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    name_long = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    formal_EN = table.Column<string>(type: "character varying(55)", maxLength: 55, nullable: true),
                    name_EN = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    Coordinates = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
