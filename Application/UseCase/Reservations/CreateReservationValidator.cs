using FluentValidation;

namespace Application.UseCase.Reservations;

public sealed class CreateReservationValidator : AbstractValidator<CreateReservation>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.ReservationCode)
            .NotEmpty()
            .MaximumLength(30)
            .Matches("^[A-Z0-9]+$").WithMessage("Reservation code must contain only uppercase letters and numbers.");

        RuleFor(x => x.ClientId).GreaterThan(0);
        RuleFor(x => x.ReservationStatusId).GreaterThan(0);
        RuleFor(x => x.TotalValue).GreaterThanOrEqualTo(0);
    }
}
