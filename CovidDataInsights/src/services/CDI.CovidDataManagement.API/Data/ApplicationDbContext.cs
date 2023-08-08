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