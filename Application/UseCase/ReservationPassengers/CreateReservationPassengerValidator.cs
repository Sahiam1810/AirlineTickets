using FluentValidation;

namespace Application.UseCase.ReservationPassengers;

public sealed class CreateReservationPassengerValidator : AbstractValidator<CreateReservationPassenger>
{
    public CreateReservationPassengerValidator()
    {
        RuleFor(x => x.ReservationFlightId).GreaterThan(0);
        RuleFor(x => x.PassengerId).GreaterThan(0);
    }
}
