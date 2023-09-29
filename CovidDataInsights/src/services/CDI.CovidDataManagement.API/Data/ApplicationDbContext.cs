using CDI.CovidDataManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.CovidDataManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // EF will not track changes made to entities
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            // Disable the automatic detection of changes made to entities
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<IntegrationModel>? IntegrationData { get; set; }
        public DbSet<VaccinationDataModel>? VaccinationData { get; set; }
        public DbSet<VaccinationMetaDataModel>? VaccinationMetaData { get; set; }
        public DbSet<WhoGlobalDataModel>? WhoGlobalData { get; set; }
        public DbSet<WhoGlobalTableDataModel>? WhoGlobalTableData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the IntegrationModel entity
            modelBuilder.Entity<IntegrationModel>(entity =>
            {
                // Set the primary key
                entity.HasKey(e => e.Id);

                // Set the column name for the Id property
                entity.Property(e => e.Id)
                .HasColumnName("IntegrationId")
                .IsRequired();

                // Configure the IntegrationTimestamp property
                entity.Property(e => e.IntegrationTimestamp)
                .HasColumnType("datetime2")
                .IsRequired();

                // Configure the FileName property
                entity.Property(e => e.FileName)
                .HasMaxLength(35)
                .IsRequired();

                // Configure the NumberOfRows property
                entity.Property(e => e.NumberOfRows)
                .HasPrecision(7)
                .IsRequired();

                // Configure the RowsIntegrated property
                entity.Property(e => e.RowsIntegrated)
                .HasPrecision(7)
                .IsRequired();

                // Create an index for the Id
                entity.HasIndex(e => e.Id).IsUnique(false).HasDatabaseName("IX_IntegrationId");

                // Create an index for the FileName
                entity.HasIndex(e => e.FileName).IsUnique(false).HasDatabaseName("IX_FileName");
            });

            // Configure the VaccinationData entity
            modelBuilder.Entity<VaccinationDataModel>(entity =>
            {
                // Set the PK
                entity.HasKey(e => e.Id);

                // Set the 1:N relationship
                entity.HasOne(v => v.IntegrationData)
                .WithMany(i => i.VaccinationData)
                .HasForeignKey(v => v.IntegrationId)
                .OnDelete(DeleteBehavior.Restrict);

                // Configure the Country property
                entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsRequired();

                // Configure the ISO3 property
                entity.Property(e => e.ISO3)
                .HasMaxLength(3)
                .IsRequired();

                // Configure the WhoRegion property
                entity.Property(e => e.WhoRegion)
                .HasMaxLength(5)
                .IsRequired();

                // Configure the DataSource property
                entity.Property(e => e.DataSource)
                .HasMaxLength(9)
                .IsRequired();

                // Configure the VaccinesUsed property
                entity.Property(e => e.VaccinesUsed)
                .HasMaxLength(3);

                // Configure the DateUpdated property
                entity.Property(e => e.DateUpdated)
                .HasMaxLength(10);

                // Configure the FirstVaccineDate property
                entity.Property(e => e.FirstVaccineDate)
                .HasMaxLength(10);

                // Create an index for the IntegrationId
                entity.HasIndex(e => e.IntegrationId).IsUnique(false).HasDatabaseName("IX_IntegrationId");

                // Create an index for the Country
                entity.HasIndex(e => e.Country).IsUnique(false).HasDatabaseName("IX_Country");
            });

            // Configure the VaccinationMetaData entity
            modelBuilder.Entity<VaccinationMetaDataModel>(entity =>
            {
                // Set the PK
                entity.HasKey(e => e.Id);

                // Set the 1:N relationship
                entity.HasOne(v => v.IntegrationData)
                .WithMany(i => i.VaccinationMetaData)
                .HasForeignKey(v => v.IntegrationId)
                .OnDelete(DeleteBehavior.Restrict);

                // Configure the ISO3 property
                entity.Property(e => e.ISO3)
                .HasMaxLength(3)
                .IsRequired();

                // Configure the VaccineName property
                entity.Property(e => e.VaccineName)
                .HasMaxLength(64)
                .IsRequired();

                // Configure the ProductName property
                entity.Property(e => e.ProductName)
                .HasMaxLength(64);

                // Configure the CompanyName property
                entity.Property(e => e.CompanyName)
                .HasMaxLength(50);

                // Configure the AuthorizationDate property
                entity.Property(e => e.AuthorizationDate)
                .HasMaxLength(10);

                // Configure the StartDate property
                entity.Property(e => e.StartDate)
                .HasMaxLength(10);

                // Configure the EndDate property
                entity.Property(e => e.EndDate)
                .HasMaxLength(10);

                // Configure the DataSource property
                entity.Property(e => e.DataSource)
                .HasMaxLength(9)
                .IsRequired();

                // Configure the DataSource property
                entity.Property(e => e.DataSource)
                .HasMaxLength(9)
                .IsRequired();

                // Configure the VaccinesUsed property
                entity.Property(e => e.Comment)
                .HasMaxLength(10);
            });

            // Configure the WhoGlobalData entity
            modelBuilder.Entity<WhoGlobalDataModel>(entity =>
            {
                // Set the PK
                entity.HasKey(e => e.Id);

                // Set the 1:N relationship
                entity.HasOne(v => v.IntegrationData)
                .WithMany(i => i.WhoGlobalData)
                .HasForeignKey(v => v.IntegrationId)
                .OnDelete(DeleteBehavior.Restrict);

                // Configure the DateReported property
                entity.Property(e => e.DateReported)
                .HasMaxLength(10)
                .IsRequired();

                // Configure the CountryCode property
                entity.Property(e => e.CountryCode)
                .HasMaxLength(3);

                // Configure the Country property
                entity.Property(e => e.Country)
                .HasMaxLength(65)
                .IsRequired();

                // Configure the WhoRegion property
                entity.Property(e => e.WhoRegion)
                .HasMaxLength(10)
                .IsRequired();
            });

            // Configure the WhoGlobalTableData entity
            modelBuilder.Entity<WhoGlobalTableDataModel>(entity =>
            {
                // Set the PK
                entity.HasKey(e => e.Id);

                // Set the 1:N relationship
                entity.HasOne(v => v.IntegrationData)
                .WithMany(i => i.WhoGlobalTableData)
                .HasForeignKey(v => v.IntegrationId)
                .OnDelete(DeleteBehavior.Restrict);

                // Configure the Name property
                entity.Property(e => e.Name)
                .HasMaxLength(60)
                .IsRequired();

                // Configure the WhoRegion property
                entity.Property(e => e.WhoRegion)
                .HasMaxLength(25);

                // Create an index for the IntegrationId
                entity.HasIndex(e => e.IntegrationId).IsUnique(false).HasDatabaseName("IX_IntegrationId");

                // Create an index for the Country
                entity.HasIndex(e => e.Name).IsUnique(false).HasDatabaseName("IX_CountryName");
            });

            // Apply configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}