using FluentValidation;

namespace Application.UseCase.CardIssuers;

public sealed class DeleteCardIssuerValidator : AbstractValidator<DeleteCardIssuer>
{
    public DeleteCardIssuerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
