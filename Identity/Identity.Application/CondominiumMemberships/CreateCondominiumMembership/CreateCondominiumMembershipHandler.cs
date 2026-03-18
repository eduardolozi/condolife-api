using Identity.Contracts.CondominiumMemberships.CreateCondominiumMembership;
using Identity.Domain.Enums;

namespace Identity.Application.CondominiumMemberships.CreateCondominiumMembership;

public class CreateCondominiumMembershipHandler(CreateCondominiumMembershipUseCase useCase) : ICreateCondominiumMembershipHandler
{
    public Task HandleAsync(int condominiumId, Guid externalUserId, CancellationToken ct)
    {
        var command = new CreateCondominiumMembershipCommand
        {
            UserRole = UserRole.Syndic,
            CondominiumId = condominiumId
        };
        
        return useCase.HandleAsync(command, externalUserId, ct);
    }
}