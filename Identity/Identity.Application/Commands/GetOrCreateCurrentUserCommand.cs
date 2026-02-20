using Identity.Domain.Entities;
using Identity.Domain.Enums;

namespace Identity.Application.Commands;

public class GetOrCreateCurrentUserCommand
{
    public Guid ExternalUserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }

    public User ToUser() => new()
    {
        ExternalId = ExternalUserId,
        FullName = FullName,
        Email = Email
    };
}