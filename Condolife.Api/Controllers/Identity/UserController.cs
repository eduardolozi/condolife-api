using Condolife.Api.Extensions;
using Identity.Application.Users.GetOrCreateCurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condolife.Api.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    [HttpPost("me")]
    public async Task<ActionResult<GetCurrentUserResult>> GetOrCreateCurrentUser(
        [FromServices] GetOrCreateCurrentUserUseCase useCase, CancellationToken ct)
    {
        try
        {
            var currentUser = new GetOrCreateCurrentUserCommand
            {
                ExternalUserId = User.GetExternalUserId(),
                Email = User.GetEmail(),
                Name = User.GetFullName()
            };
            var user = await useCase.HandleAsync(currentUser, ct);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}