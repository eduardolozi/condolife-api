using Identity.Application.Abstractions;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.CondominiumMemberships.CreateCondominiumMembership;

public class CreateCondominiumMembershipUseCase(IIdentityDbContext dbContext)
{
    public async Task HandleAsync(CreateCondominiumMembershipCommand command, Guid externalUserId, CancellationToken ct)
    {
        var userId = await dbContext
            .Users
            .Where(x => x.ExternalId == externalUserId)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(ct);

        if (userId == 0) throw new Exception("O usuário solicitante não foi encontrado.");
        
        var membership = new CondominiumMembership
        {
            CondominiumId = command.CondominiumId,
            JoinDate = DateTime.UtcNow,
            UserId = userId,
            UserRole = command.UserRole
        };
        
        await dbContext.CondominiumMemberships.AddAsync(membership, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}