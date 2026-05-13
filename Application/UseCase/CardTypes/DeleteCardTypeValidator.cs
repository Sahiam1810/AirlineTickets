using FluentValidation;

namespace Application.UseCase.CardTypes;

public sealed class DeleteCardTypeValidator : AbstractValidator<DeleteCardType>
{
    public DeleteCardTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
