using FluentValidation;

namespace Application.UseCase.ReservationFlights;

public sealed class UpdateReservationFlightValidator : AbstractValidator<UpdateReservationFlight>
{
    public UpdateReservationFlightValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ReservationId).GreaterThan(0);
        RuleFor(x => x.FlightId).GreaterThan(0);
        RuleFor(x => x.PartialValue).GreaterThanOrEqualTo(0);
    }
}
