using FluentValidation;

namespace Application.UseCase.CardIssuers;

public sealed class CreateCardIssuerValidator : AbstractValidator<CreateCardIssuer>
{
    public CreateCardIssuerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
