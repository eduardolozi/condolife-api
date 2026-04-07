namespace Condominiums.Application.Management.GetResidentPreRegistrationTemplate;

public record GetResidentPreRegistrationTemplateResult(
    byte[] Content,
    string FileName,
    string ContentType);
