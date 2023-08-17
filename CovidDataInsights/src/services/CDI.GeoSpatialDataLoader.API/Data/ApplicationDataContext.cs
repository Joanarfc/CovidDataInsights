using CDI.GeoSpatialDataLoader.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CDI.GeoSpatialDataLoader.API.Data
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
            // EF will not track changes made to entities
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            // Disable the automatic detection of changes made to entities
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<GeoSpatialModel>? Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeoSpatialModel>(entity =>
            {
                entity.Property(e => e.Featurecla)
                .HasColumnName("featurecla")
                .HasMaxLength(20)
                .IsRequired();

                entity.Property(e => e.Sovereignt)
                .HasColumnName("sovereignt")
                .HasMaxLength(35)
                .IsRequired();

                entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasMaxLength(20)
                .IsRequired();

                entity.Property(e => e.Admin)
                .HasColumnName("admin")
                .HasMaxLength(40)
                .IsRequired();

                entity.Property(e => e.NameLong)
                .HasColumnName("name_long")
                .HasMaxLength(40)
                .IsRequired();

                entity.Property(e => e.FormalEN)
                .HasColumnName("formal_EN")
                .HasMaxLength(55);

                entity.Property(e => e.NameEN)
                .HasColumnName("name_EN")
                .HasMaxLength(45)
                .IsRequired();
            });
        }
    }
}