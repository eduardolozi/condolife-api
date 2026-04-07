using Condominiums.Application.Abstractions;
using Condominiums.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Infrastructure;

public class CondominiumDbContext(DbContextOptions<CondominiumDbContext> options) : DbContext(options), ICondominiumDbContext
{
    private const string SchemaName = "condominium";

    public DbSet<Condominium> Condominiums { get; set; }
    public DbSet<ResidentProspect> ResidentProspects { get; set; }
    public DbSet<Tower> Towers { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);
        modelBuilder.HasPostgresExtension("citext");

        modelBuilder.Entity<State>(x =>
        {
            x.HasIndex(s => s.Code).IsUnique();
        });

        modelBuilder.Entity<City>(x =>
        {
            x.HasIndex(c => c.IbgeCode, "Index_Cities_IbgeCode")
                .IsUnique();
        });

        modelBuilder.Entity<ResidentProspect>(x =>
        {
            x.Property(rp => rp.Name)
                .HasMaxLength(200);

            x.Property(rp => rp.Cpf)
                .HasMaxLength(11);

            x.Property(rp => rp.Apartment)
                .HasMaxLength(50)
                .HasColumnType("citext");

            x.Property(rp => rp.Block)
                .HasMaxLength(50)
                .HasColumnType("citext");

            x.HasIndex(rp => new { rp.CondominiumId, rp.Cpf })
                .HasDatabaseName("UX_ResidentProspects_CondominiumId_Cpf")
                .IsUnique();

            x.HasIndex(rp => new { rp.CondominiumId, rp.Apartment })
                .HasDatabaseName("UX_ResidentProspects_CondominiumId_Apartment_WithoutBlock")
                .HasFilter("\"Block\" IS NULL")
                .IsUnique();

            x.HasIndex(rp => new { rp.CondominiumId, rp.Apartment, rp.Block })
                .HasDatabaseName("UX_ResidentProspects_CondominiumId_Apartment_Block_WithBlock")
                .HasFilter("\"Block\" IS NOT NULL")
                .IsUnique();

            x.HasOne(rp => rp.Condominium)
                .WithMany(c => c.ResidentProspects)
                .HasForeignKey(rp => rp.CondominiumId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
