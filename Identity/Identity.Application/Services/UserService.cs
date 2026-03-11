using Identity.Application.DTOs;
using Identity.Application.Interfaces.InfraAbstractions;
using Identity.Application.Interfaces.Services;
using Identity.Application.Responses;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Identity.Application.Services;

public class UserService(IIdentityDbContext dbContext) : IUserService
{
    public Task<GetCurrentUserResponse?> FindCurrentUserByExternalId(Guid externalUserId, CancellationToken ct)
    {
        return dbContext
            .Users
            .Where(u => u.ExternalId == externalUserId)
            .Select(u => new GetCurrentUserResponse(u.Id, u.ExternalId, u.Name, u.Email)
            {
                AvatarUrl =  u.AvatarUrl,
                Condominiums = u.Condominiums
                    .Select(c => new CondominiumMembershipDto(c.CondominiumId, c.Role))
                    .ToList()
            })
            .SingleOrDefaultAsync(cancellationToken: ct);
    }

    public async Task<GetCurrentUserResponse> CreateUser(User user, CancellationToken ct)
    {
        await dbContext.Users.AddAsync(user, cancellationToken: ct);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken: ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" } pgEx)
        {
            throw pgEx.ConstraintName switch
            {
                "UX_users_email" => new Exception("Email já cadastrado."),
                "UX_users_external_id" => new Exception("Usuário já existe (ExternalId duplicado)."),
                _ => new Exception("Violação de unicidade.")
            };
        }
        
        return new GetCurrentUserResponse(user.Id, user.ExternalId, user.Name, user.Email);
    }
}