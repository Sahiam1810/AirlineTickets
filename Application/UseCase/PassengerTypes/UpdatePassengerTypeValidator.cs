using FluentValidation;

namespace Application.UseCase.PassengerTypes;

public sealed class UpdatePassengerTypeValidator : AbstractValidator<UpdatePassengerType>
{
    public UpdatePassengerTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50);

        RuleFor(x => x.AgeMin)
            .GreaterThanOrEqualTo(0)
            .When(x => x.AgeMin.HasValue)
            .WithMessage("La edad minima no puede ser negativa.");

        RuleFor(x => x.AgeMax)
            .GreaterThanOrEqualTo(0)
            .When(x => x.AgeMax.HasValue)
            .WithMessage("La edad maxima no puede ser negativa.");

        RuleFor(x => x)
            .Must(x => !x.AgeMin.HasValue || !x.AgeMax.HasValue || x.AgeMin.Value <= x.AgeMax.Value)
            .WithMessage("La edad minima no puede ser mayor que la edad maxima.");
    }
}
