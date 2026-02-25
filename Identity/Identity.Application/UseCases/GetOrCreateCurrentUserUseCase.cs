using FluentValidation;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Application.Responses;
using Identity.Application.Validators;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.UseCases;

public class GetOrCreateCurrentUserUseCase(IIdentityDbContext dbContext, IValidator<GetOrCreateCurrentUserCommand> validator)
{
    public async Task<GetCurrentUserResponse> HandleAsync(GetOrCreateCurrentUserCommand command)
    {
        var currentUser = await FindUser(command.ExternalUserId);
        // TO DO: BUSCAR INFOS DO CONDOMINIO
        if (currentUser != null) return currentUser;

        return await CreateUser(command);
    }

    private Task<GetCurrentUserResponse?> FindUser(Guid externalUserId)
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
            .SingleOrDefaultAsync();
    }

    private async Task<GetCurrentUserResponse> CreateUser(GetOrCreateCurrentUserCommand command)
    {
        await validator.ValidateAndThrowAsync(command);
        
        var userEntity = command.ToUser();
        await dbContext.Users.AddAsync(userEntity);
        await dbContext.SaveChangesAsync();
        
        return new GetCurrentUserResponse(userEntity.Id, userEntity.ExternalId, userEntity.Name, userEntity.Email);
    }
}