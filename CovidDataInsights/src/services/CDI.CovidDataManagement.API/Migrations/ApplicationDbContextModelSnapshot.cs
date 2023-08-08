﻿// <auto-generated />
using System;
using CDI.CovidDataManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CDI.CovidDataManagement.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.IntegrationModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("IntegrationId");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<DateTime>("IntegrationTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("NumberOfRows")
                        .HasPrecision(7)
                        .HasColumnType("int");

                    b.Property<int>("RowsIntegrated")
                        .HasPrecision(7)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("IntegrationData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.VaccinationDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DataSource")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("DateUpdated")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("FirstVaccineDate")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ISO3")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<Guid>("IntegrationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("NumberVaccinesTypesUsed")
                        .HasColumnType("int");

                    b.Property<long?>("PersonsBoosterAddDose")
                        .HasColumnType("bigint");

                    b.Property<double?>("PersonsBoosterAddDose_Per100")
                        .HasColumnType("float");

                    b.Property<long?>("PersonsLastDose")
                        .HasColumnType("bigint");

                    b.Property<double?>("PersonsLastDosePer100")
                        .HasColumnType("float");

                    b.Property<long?>("PersonsVaccinated_1Plus_Dose")
                        .HasColumnType("bigint");

                    b.Property<double?>("PersonsVaccinated_1Plus_Dose_Per100")
                        .HasColumnType("float");

                    b.Property<long?>("TotalVaccinations")
                        .HasColumnType("bigint");

                    b.Property<double?>("TotalVaccinations_Per100")
                        .HasColumnType("float");

                    b.Property<string>("VaccinesUsed")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("WhoRegion")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId");

                    b.ToTable("VaccinationData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.VaccinationMetaDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuthorizationDate")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Comment")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CompanyName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DataSource")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("EndDate")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("ISO3")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<Guid>("IntegrationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductName")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("StartDate")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("VaccineName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId");

                    b.ToTable("VaccinationMetaData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.WhoGlobalDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(65)
                        .HasColumnType("nvarchar(65)");

                    b.Property<string>("CountryCode")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<long?>("CumulativeCases")
                        .HasColumnType("bigint");

                    b.Property<long?>("CumulativeDeaths")
                        .HasColumnType("bigint");

                    b.Property<string>("DateReported")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<Guid>("IntegrationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("NewCases")
                        .HasColumnType("bigint");

                    b.Property<long?>("NewDeaths")
                        .HasColumnType("bigint");

                    b.Property<string>("WhoRegion")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationId");

                    b.ToTable("WhoGlobalData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.VaccinationDataModel", b =>
                {
                    b.HasOne("CDI.CovidDataManagement.API.Models.IntegrationModel", "IntegrationData")
                        .WithMany("VaccinationData")
                        .HasForeignKey("IntegrationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IntegrationData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.VaccinationMetaDataModel", b =>
                {
                    b.HasOne("CDI.CovidDataManagement.API.Models.IntegrationModel", "IntegrationData")
                        .WithMany("VaccinationMetaData")
                        .HasForeignKey("IntegrationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IntegrationData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.WhoGlobalDataModel", b =>
                {
                    b.HasOne("CDI.CovidDataManagement.API.Models.IntegrationModel", "IntegrationData")
                        .WithMany("WhoGlobalData")
                        .HasForeignKey("IntegrationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IntegrationData");
                });

            modelBuilder.Entity("CDI.CovidDataManagement.API.Models.IntegrationModel", b =>
                {
                    b.Navigation("VaccinationData");

                    b.Navigation("VaccinationMetaData");

                    b.Navigation("WhoGlobalData");
                });
#pragma warning restore 612, 618
        }
    }
}
