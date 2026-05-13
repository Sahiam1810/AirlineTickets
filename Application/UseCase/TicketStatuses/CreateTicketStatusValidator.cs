using FluentValidation;

namespace Application.UseCase.TicketStatuses;

public sealed class CreateTicketStatusValidator : AbstractValidator<CreateTicketStatus>
{
    public CreateTicketStatusValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
