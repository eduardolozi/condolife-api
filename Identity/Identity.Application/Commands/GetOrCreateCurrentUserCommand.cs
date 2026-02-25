using Identity.Domain.Entities;

namespace Identity.Application.Commands;

public class GetOrCreateCurrentUserCommand
{
    public Guid ExternalUserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public User ToUser()
    {
        return new User
        {
            ExternalId = ExternalUserId,
            Name = Name,
            Email = Email
        };
    }
}