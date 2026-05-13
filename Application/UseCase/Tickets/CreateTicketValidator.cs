using FluentValidation;

namespace Application.UseCase.Tickets;

public sealed class CreateTicketValidator : AbstractValidator<CreateTicket>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.ReservationPassengerId).GreaterThan(0);
        RuleFor(x => x.TicketCode)
            .NotEmpty()
            .MaximumLength(30);
        RuleFor(x => x.IssuedAt).NotEmpty();
        RuleFor(x => x.TicketStatusId).GreaterThan(0);
    }
}
