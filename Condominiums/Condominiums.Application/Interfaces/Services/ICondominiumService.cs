using Condominiums.Domain.Entities;

namespace Condominiums.Application.Interfaces.Services;

public interface ICondominiumService
{
    Task Create(Condominium condominium, CancellationToken ct);
}