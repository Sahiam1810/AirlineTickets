using FluentValidation;

namespace Application.UseCase.CheckIns;

public sealed class UpdateCheckInValidator : AbstractValidator<UpdateCheckIn>
{
    public UpdateCheckInValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.TicketId).GreaterThan(0);
        RuleFor(x => x.StaffId).GreaterThan(0);
        RuleFor(x => x.FlightSeatId).GreaterThan(0);
        RuleFor(x => x.CheckInDate).NotEmpty();
        RuleFor(x => x.CheckInStatusId).GreaterThan(0);
        RuleFor(x => x.BoardingPassNumber)
            .NotEmpty()
            .MaximumLength(20);
        RuleFor(x => x.CheckedBaggageWeightKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CheckedBaggageWeightKg)
            .Equal(0)
            .When(x => !x.HasCheckedBaggage)
            .WithMessage("Checked baggage weight must be 0 when there is no checked baggage.");
    }
}
