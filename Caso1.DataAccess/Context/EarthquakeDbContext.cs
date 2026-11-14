using Caso1.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caso1.DataAccess.Context;

public class EarthquakeDbContext : DbContext
{
    public EarthquakeDbContext(DbContextOptions<EarthquakeDbContext> options)
        : base(options)
    {
    }

    // DbSet representa la tabla Earthquakes de SQL Server.
    public DbSet<Earthquake> Earthquakes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Earthquake>()
            .HasIndex(earthquake => earthquake.ExternalId)
            .IsUnique();

        modelBuilder.Entity<Earthquake>()
            .Property(earthquake => earthquake.Magnitude)
            .HasPrecision(5, 2);
    }
}
