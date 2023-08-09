using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CDI.CovidDataManagement.API.Migrations
{
    public partial class CorrectedWhoGlobalTableData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DeathsNewlyReportedInLast7Days_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "DeathsNewlyReportedInLast7Days",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DeathsNewlyReportedInLast24Hours",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "DeathsCumulativeTotal_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<long>(
                name: "DeathsCumulativeTotal",
                table: "WhoGlobalTableData",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "CasesNewlyReportedInLast7Days_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "CasesNewlyReportedInLast7Days",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CasesNewlyReportedInLast24Hours",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "CasesCumulativeTotal_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<long>(
                name: "CasesCumulativeTotal",
                table: "WhoGlobalTableData",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "DeathsNewlyReportedInLast7Days_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeathsNewlyReportedInLast7Days",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeathsNewlyReportedInLast24Hours",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "DeathsCumulativeTotal_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeathsCumulativeTotal",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CasesNewlyReportedInLast7Days_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CasesNewlyReportedInLast7Days",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CasesNewlyReportedInLast24Hours",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CasesCumulativeTotal_Per100000_Population",
                table: "WhoGlobalTableData",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CasesCumulativeTotal",
                table: "WhoGlobalTableData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
