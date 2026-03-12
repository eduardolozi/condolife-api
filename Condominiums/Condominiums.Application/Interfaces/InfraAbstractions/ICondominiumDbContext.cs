using Condominiums.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Application.Interfaces.InfraAbstractions;

public interface ICondominiumDbContext
{
    public DbSet<Condominium> Condominiums { get; set; }
    public DbSet<Tower> Towers { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}