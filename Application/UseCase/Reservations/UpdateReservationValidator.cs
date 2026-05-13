using FluentValidation;

namespace Application.UseCase.Reservations;

public sealed class UpdateReservationValidator : AbstractValidator<UpdateReservation>
{
    public UpdateReservationValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ReservationStatusId).GreaterThan(0);
        RuleFor(x => x.TotalValue).GreaterThanOrEqualTo(0);
    }
}
