using FluentValidation;

namespace Application.UseCase.ReservationPassengers;

public sealed class UpdateReservationPassengerValidator : AbstractValidator<UpdateReservationPassenger>
{
    public UpdateReservationPassengerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ReservationFlightId).GreaterThan(0);
        RuleFor(x => x.PassengerId).GreaterThan(0);
    }
}
