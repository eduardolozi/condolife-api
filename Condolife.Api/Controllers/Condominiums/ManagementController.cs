using Condominiums.Application.Management.GetResidentPreRegistrationTemplate;
using Microsoft.AspNetCore.Authorization;
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
}
