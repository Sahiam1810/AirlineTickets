using FluentValidation;

namespace Application.UseCase.Flights;

public sealed class ChangeFlightStateValidator : AbstractValidator<ChangeFlightState>
{
    public ChangeFlightStateValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x.FlightStateId).GreaterThan(0);
    }
}
