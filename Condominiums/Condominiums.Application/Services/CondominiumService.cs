using Condominiums.Application.Interfaces.InfraAbstractions;
using Condominiums.Application.Interfaces.Services;
using Condominiums.Domain.Entities;

namespace Condominiums.Application.Services;

public class CondominiumService(ICondominiumDbContext dbContext) : ICondominiumService
{
    public async Task Create(Condominium condominium, CancellationToken ct)
    {
        await dbContext.Condominiums.AddAsync(condominium, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}