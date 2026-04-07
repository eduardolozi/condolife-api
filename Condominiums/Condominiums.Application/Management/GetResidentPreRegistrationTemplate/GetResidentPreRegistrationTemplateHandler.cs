using Condominiums.Application.Abstractions;

namespace Condominiums.Application.Management.GetResidentPreRegistrationTemplate;

public class GetResidentPreRegistrationTemplateHandler(ICsvFileService csvFileService)
{
    private static readonly string[] Header =
    [
        "Nome",
        "Cpf",
        "Apartamento",
        "Bloco"
    ];

    public GetResidentPreRegistrationTemplateResult Handle(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var content = csvFileService.GenerateEmptyFileWithHeader(Header);

        return new GetResidentPreRegistrationTemplateResult(
            content,
            "resident-pre-registration-template.csv",
            "text/csv"
        );
    }
}
