using Condolife.Api.Extensions;
using Identity.Application.CondominiumMemberships.GetCondominiumMemberships;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condolife.Api.Controllers.Identity;

[ApiController]
[Route("api/{controller}")]
[Authorize]
public class CondominiumMembershipController : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> Get(
        [FromServices] GetCondominiumMembershipsUseCase useCase,
        CancellationToken ct)
    {
        var externalUserId = User.GetExternalUserId();
        var membershipsInfos = await useCase.HandleAsync(externalUserId, ct);
        return Ok(membershipsInfos);
    }
}