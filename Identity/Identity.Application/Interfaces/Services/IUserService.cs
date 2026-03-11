using Identity.Application.Responses;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Services;

public interface IUserService
{
    Task<GetCurrentUserResponse?> FindCurrentUserByExternalId(Guid externalId, CancellationToken ct);
    Task<GetCurrentUserResponse> CreateUser(User user, CancellationToken ct);
}