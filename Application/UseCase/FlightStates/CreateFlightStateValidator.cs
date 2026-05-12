using FluentValidation;

namespace Application.UseCase.FlightStates;

public sealed class CreateFlightStateValidator : AbstractValidator<CreateFlightState>
{
    public CreateFlightStateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50);
    }
}
