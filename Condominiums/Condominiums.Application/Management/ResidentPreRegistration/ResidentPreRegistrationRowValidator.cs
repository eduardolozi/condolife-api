using FluentValidation;

namespace Condominiums.Application.Management.ResidentPreRegistration;

public class ResidentPreRegistrationRowValidator : AbstractValidator<ResidentPreRegistrationRow>
{
    private const string InvalidCpfMessage = "O CPF deve estar em um dos formatos permitidos: xxx.xxx.xxx-xx, xxxxxxxxx-xx ou xxxxxxxxxxx.";

    public ResidentPreRegistrationRowValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Must(ResidentPreRegistrationNormalizer.HasAllowedCpfFormat).WithMessage(InvalidCpfMessage)
            .Must(cpf => ResidentPreRegistrationNormalizer.NormalizeCpf(cpf).Length == 11).WithMessage(InvalidCpfMessage);

        RuleFor(x => x.Apartamento)
            .NotEmpty().WithMessage("O apartamento é obrigatório.");
    }
}
