namespace Identity.Contracts.CondominiumMemberships.CreateCondominiumMembership;

public interface ICreateCondominiumMembershipHandler
{
    Task HandleAsync(int condominiumId, Guid externalUserId, CancellationToken ct);
}