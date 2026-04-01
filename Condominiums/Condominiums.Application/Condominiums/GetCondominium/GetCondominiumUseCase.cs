using Condominiums.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Condominiums.Application.Condominiums.GetCondominium
{
    public class GetCondominiumUseCase(ICondominiumDbContext dbContext)
    {
        public Task<GetCondominiumResult?> HandleAsync(int condominiumId, CancellationToken ct)
        {
            return dbContext.Condominiums
                .Where(x => x.Id == condominiumId)
                .Select(x => new GetCondominiumResult(x.Name))
                .SingleOrDefaultAsync(ct);
        }
    }
}
