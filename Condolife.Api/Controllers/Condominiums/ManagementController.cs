using Condominiums.Application.Management.GetResidentPreRegistrationTemplate;
using Condominiums.Application.Management.ResidentPreRegistration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Condolife.Api.Controllers.Condominiums;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ManagementController : ControllerBase
{
    [HttpGet("resident-pre-registration-template")]
    public ActionResult GetResidentPreRegistrationTemplate(
        [FromServices] GetResidentPreRegistrationTemplateHandler handler,
        CancellationToken ct)
    {
        var file = handler.Handle(ct);
        return File(file.Content, file.ContentType, file.FileName);
    }

    [HttpPost("resident-pre-registration/{condominiumId:int}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ImportResidentPreRegistrationResult>> ImportResidentPreRegistration(
        [FromRoute] int condominiumId,
        [FromForm] IFormFile? file,
        [FromServices] ImportResidentPreRegistrationUseCase useCase,
        CancellationToken ct)
    {
        await using var stream = new MemoryStream();
        if (file is not null)
        {
            await file.CopyToAsync(stream, ct);
        }

        var result = await useCase.HandleAsync(new ImportResidentPreRegistrationCommand
        {
            CondominiumId = condominiumId,
            FileName = file?.FileName ?? string.Empty,
            Content = stream.ToArray()
        }, ct);

        return Ok(result);
    }
}
