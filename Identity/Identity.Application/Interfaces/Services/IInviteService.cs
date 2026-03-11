using Identity.Domain.Entities;
using Identity.Domain.Enums;

namespace Identity.Application.Interfaces.Services;

public interface IInviteService
{
    Task<bool> PendingInviteExistsFor(string email, UserRole userRole, CancellationToken ct);
    Task CreateInvite(Invite invite, CancellationToken ct);
}