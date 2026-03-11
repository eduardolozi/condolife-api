using Identity.Application.Interfaces.InfraAbstractions;
using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Services;

public class InviteService(IIdentityDbContext dbContext) : IInviteService
{
    public Task<bool> PendingInviteExistsFor(string email, UserRole userRole, CancellationToken ct)
    {
        return dbContext.Invites.AnyAsync(x => 
            EF.Functions.Like(email, x.GuestEmail) && x.IsPending && x.Role == userRole,
            cancellationToken: ct
        );
    }

    public async Task CreateInvite(Invite invite, CancellationToken ct)
    {
        await dbContext.Invites.AddAsync(invite, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}