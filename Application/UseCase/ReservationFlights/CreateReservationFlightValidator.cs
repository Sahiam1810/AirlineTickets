using FluentValidation;

namespace Application.UseCase.ReservationFlights;

public sealed class CreateReservationFlightValidator : AbstractValidator<CreateReservationFlight>
{
    public CreateReservationFlightValidator()
    {
        RuleFor(x => x.ReservationId).GreaterThan(0);
        RuleFor(x => x.FlightId).GreaterThan(0);
        RuleFor(x => x.PartialValue).GreaterThanOrEqualTo(0);
    }
}
