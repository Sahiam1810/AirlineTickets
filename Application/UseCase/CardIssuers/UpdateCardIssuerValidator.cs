using FluentValidation;

namespace Application.UseCase.CardIssuers;

public sealed class UpdateCardIssuerValidator : AbstractValidator<UpdateCardIssuer>
{
    public UpdateCardIssuerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
