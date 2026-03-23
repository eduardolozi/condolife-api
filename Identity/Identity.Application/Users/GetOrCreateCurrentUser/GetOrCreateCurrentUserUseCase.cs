using FluentValidation;
using Identity.Application.Abstractions;
using Identity.Application.CondominiumMemberships.GetCondominiumMemberships;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using Identity.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Identity.Application.Users.GetOrCreateCurrentUser;

public class GetOrCreateCurrentUserUseCase(
    IIdentityDbContext dbContext,
    IValidator<GetOrCreateCurrentUserCommand> validator)
{
    public async Task<GetCurrentUserResult> HandleAsync(GetOrCreateCurrentUserCommand command, CancellationToken ct)
    {
        var currentUser = await GetCurrentUser(command.ExternalUserId, ct);
        if (currentUser is not null) return currentUser;
        
        await validator.ValidateAndThrowAsync(command, ct);
        var user = command.ToUser();

        await CreateUser(user, ct);
        
        return new GetCurrentUserResult(user.Id, user.ExternalId, user.Name, user.Email);
    }

    private Task<GetCurrentUserResult?> GetCurrentUser(Guid externalUserId, CancellationToken ct)
    {
        return dbContext
            .Users
            .Where(u => u.ExternalId == externalUserId)
            .Select(u => new GetCurrentUserResult(u.Id, u.ExternalId, u.Name, u.Email)
            {
                AvatarUrl =  u.AvatarUrl
            })
            .SingleOrDefaultAsync(cancellationToken: ct);
    }
    
    private async Task CreateUser(User user, CancellationToken ct)
    {
        await dbContext.Users.AddAsync(user, cancellationToken: ct);
        try
        {
            await dbContext.SaveChangesAsync(cancellationToken: ct);
        }
        catch (DbUpdateException ex) when (DbExceptionMapper.TryMap(ex, out var mappedException))
        {
            throw mappedException;
        }
    }
}