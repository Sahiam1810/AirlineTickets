using FluentValidation;

namespace Application.UseCase.FlightStates;

public sealed class UpdateFlightStateValidator : AbstractValidator<UpdateFlightState>
{
    public UpdateFlightStateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50);
    }
}
