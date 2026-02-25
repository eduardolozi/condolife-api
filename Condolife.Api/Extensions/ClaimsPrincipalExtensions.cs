using System.Security.Claims;

namespace Condolife.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public Guid GetExternalUserId()
        {
            var sub = user.FindFirstValue("sub");
            return string.IsNullOrWhiteSpace(sub) ? throw new UnauthorizedAccessException("Claim 'sub' não encontrada.") : Guid.Parse(sub);
        }

        public string GetEmail()
        {
            var email = user.FindFirstValue("email");
            return string.IsNullOrWhiteSpace(email) ? throw new UnauthorizedAccessException("Claim 'email' não encontrada.")  : email;
        }

        public string GetFullName()
        {
            var name = user.FindFirstValue("name");
            return string.IsNullOrWhiteSpace(name) ? throw new UnauthorizedAccessException("Claim 'name' não encontrada.") : name;
        }
    }
}