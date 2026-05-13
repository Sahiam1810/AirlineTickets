using FluentValidation;

namespace Application.UseCase.CardTypes;

public sealed class UpdateCardTypeValidator : AbstractValidator<UpdateCardType>
{
    public UpdateCardTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
