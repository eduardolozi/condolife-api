using Condolife.Api.Extensions;
using Identity.Application.Commands;
using Identity.Application.Responses;
using Identity.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condolife.Api.Controllers.Identity;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    [HttpPost("me")]
    public async Task<ActionResult<GetCurrentUserResponse>> GetOrCreateCurrrentUser([FromServices] GetOrCreateCurrentUserUseCase useCase)
    {
        var currentUser = new GetOrCreateCurrentUserCommand
        {
            ExternalUserId = User.GetExternalUserId(),
            Email = User.GetEmail(),
            Name =  User.GetFullName()
        };
        var user = await useCase.HandleAsync(currentUser);
        return Ok(user);
    }
}