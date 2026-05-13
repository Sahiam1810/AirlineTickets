using FluentValidation;

namespace Application.UseCase.Tickets;

public sealed class UpdateTicketValidator : AbstractValidator<UpdateTicket>
{
    public UpdateTicketValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ReservationPassengerId).GreaterThan(0);
        RuleFor(x => x.TicketCode)
            .NotEmpty()
            .MaximumLength(30);
        RuleFor(x => x.IssuedAt).NotEmpty();
        RuleFor(x => x.TicketStatusId).GreaterThan(0);
    }
}
