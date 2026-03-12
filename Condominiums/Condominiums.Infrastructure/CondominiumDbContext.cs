using Condominiums.Application.Interfaces.InfraAbstractions;
using Condominiums.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Infrastructure;

public class CondominiumDbContext(DbContextOptions<CondominiumDbContext> options) : DbContext(options), ICondominiumDbContext
{
    private const string SchemaName =  "condominium";
    
    public DbSet<Condominium> Condominiums { get; set; }
    public DbSet<Tower> Towers { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(SchemaName);

        modelBuilder.Entity<State>(x =>
        {
            x.HasIndex(s => s.Code).IsUnique();
        });
        
        modelBuilder.Entity<City>(x =>
        {
            x.HasIndex(c => c.IbgeCode, "Index_Cities_IbgeCode")
                .IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}