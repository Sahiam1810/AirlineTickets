using FluentValidation;

namespace Application.UseCase.CardTypes;

public sealed class CreateCardTypeValidator : AbstractValidator<CreateCardType>
{
    public CreateCardTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
