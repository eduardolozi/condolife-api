using FluentValidation;

namespace Condominiums.Application.Management.ResidentPreRegistration;

public class ImportResidentPreRegistrationCommandValidator : AbstractValidator<ImportResidentPreRegistrationCommand>
{
    private static readonly string[] AllowedExtensions = [".csv", ".txt"];

    public ImportResidentPreRegistrationCommandValidator()
    {
        RuleFor(x => x.CondominiumId)
            .GreaterThan(0).WithMessage("O condomínio deve ser informado.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("O arquivo deve ser informado.")
            .Must(HaveAllowedExtension).WithMessage("O arquivo deve ser do tipo CSV ou TXT.");

        RuleFor(x => x.Content)
            .NotNull().WithMessage("O arquivo deve ser informado.")
            .Must(content => content.Length > 0).WithMessage("O arquivo deve possuir conteúdo.");
    }

    private static bool HaveAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        return AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
    }
}
