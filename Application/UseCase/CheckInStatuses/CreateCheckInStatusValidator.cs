using FluentValidation;

namespace Application.UseCase.CheckInStatuses;

public sealed class CreateCheckInStatusValidator : AbstractValidator<CreateCheckInStatus>
{
    public CreateCheckInStatusValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
