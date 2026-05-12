using FluentValidation;

namespace Application.UseCase.CabinTypes;

public sealed class UpdateCabinTypeValidator : AbstractValidator<UpdateCabinType>
{
    public UpdateCabinTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Cabin type name is required.")
            .MaximumLength(50);
    }
}
