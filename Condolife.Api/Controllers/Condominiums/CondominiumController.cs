using Condolife.Api.Extensions;
using Condominiums.Application.Condominiums.CreateCondominium;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Condolife.Api.Controllers.Condominiums;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CondominiumController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateCondominiumUseCase useCase, 
        [FromBody] CreateCondominiumCommand command,
        CancellationToken ct)
    {
        var externalUserId = User.GetExternalUserId();
        await useCase.HandleAsync(command, externalUserId, ct);
        return Created();
    }
}