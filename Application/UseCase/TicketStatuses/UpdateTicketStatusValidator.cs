using FluentValidation;

namespace Application.UseCase.TicketStatuses;

public sealed class UpdateTicketStatusValidator : AbstractValidator<UpdateTicketStatus>
{
    public UpdateTicketStatusValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
