using FluentValidation;

namespace Application.UseCase.CabinTypes;

public sealed class CreateCabinTypeValidator : AbstractValidator<CreateCabinType>
{
    public CreateCabinTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Cabin type name is required.")
            .MaximumLength(50);
    }
}
