using FluentValidation;
using Identity.Application.Commands;

namespace Identity.Application.Validators;

public class GetOrCreateCurrentUserCommandValidator : AbstractValidator<GetOrCreateCurrentUserCommand>
{
    public GetOrCreateCurrentUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Continue;

        RuleFor(x => x.ExternalUserId)
            .NotEmpty().WithMessage("O externalId do usuário deve ser informado");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do usuário deve ser informado.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email do usuário deve ser informado.");
    }
}