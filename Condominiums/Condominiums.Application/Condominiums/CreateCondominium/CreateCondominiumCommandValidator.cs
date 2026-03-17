using FluentValidation;

namespace Condominiums.Application.Condominiums.CreateCondominium;

public class CreateCondominiumCommandValidator : AbstractValidator<CreateCondominiumCommand>
{
    public CreateCondominiumCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CondominiumName)
            .NotEmpty().WithMessage("O nome do condomínio deve ser informado.")
            .MaximumLength(100).WithMessage("O nome do condomínio deve possuir até 100 caracteres.");

        RuleFor(x => x.IbgeCode)
            .NotEmpty().WithMessage("O Código IBGE deve ser informado.")
            .Length(7).WithMessage("O Código IBGE deve possuir 7 dígitos.")
            .Matches(@"^\d+$").WithMessage("O Código IBGE deve possuir apenas dígitos.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("A cidade deve ser informada.")
            .MaximumLength(200).WithMessage("A cidade deve possuir até 200 caracteres.");

        RuleFor(x => x.StateCode)
            .NotEmpty().WithMessage("A UF deve ser informada.")
            .Length(2).WithMessage("A UF deve possuir 2 caracteres.");

        RuleFor(x => x.Neighborhood)
            .NotEmpty().WithMessage("O bairro deve ser informado.")
            .MaximumLength(200).WithMessage("O bairro deve possuir até 200 caracteres.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("O número de endereço deve ser informado.")
            .MaximumLength(20).WithMessage("O número de endereço deve possuir até 20 caracteres.");
        
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("A rua deve ser informada.")
            .MaximumLength(200).WithMessage("A rua deve possir até 200 caracteres.");
        
        RuleFor(x => x.PostalCode)
            .NotEmpty().WithMessage("O CEP deve ser informado.")
            .Length(8).WithMessage("O CEP deve possuir 8 dígitos.")
            .Matches(@"^\d+$").WithMessage("O CEP deve possuir apenas dígitos.");
    }
}