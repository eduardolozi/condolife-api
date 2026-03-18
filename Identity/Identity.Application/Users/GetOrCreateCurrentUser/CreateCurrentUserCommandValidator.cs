using FluentValidation;

namespace Identity.Application.Users.GetOrCreateCurrentUser;

public class CreateCurrentUserCommandValidator : AbstractValidator<GetOrCreateCurrentUserCommand>
{
    public CreateCurrentUserCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ExternalUserId)
            .NotEmpty().WithMessage("O externalId do usuário deve ser informado");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do usuário deve ser informado.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email do usuário deve ser informado.");
    }
}