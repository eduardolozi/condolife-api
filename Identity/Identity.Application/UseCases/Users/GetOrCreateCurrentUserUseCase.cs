using FluentValidation;
using Identity.Application.Commands;
using Identity.Application.Interfaces.Services;
using Identity.Application.Responses;

namespace Identity.Application.UseCases.Users;

public class GetOrCreateCurrentUserUseCase(IUserService userService, IValidator<GetOrCreateCurrentUserCommand> validator)
{
    public async Task<GetCurrentUserResponse> HandleAsync(GetOrCreateCurrentUserCommand command, CancellationToken ct)
    {
        var currentUser = await userService.FindCurrentUserByExternalId(command.ExternalUserId, ct);
        if (currentUser is not null) return currentUser;
        
        await validator.ValidateAndThrowAsync(command, ct);
        var userEntity = command.ToUser();
        
        return await userService.CreateUser(userEntity, ct);
    }
}