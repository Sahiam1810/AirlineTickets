using FluentValidation;

namespace Application.UseCase.CheckInStatuses;

public sealed class UpdateCheckInStatusValidator : AbstractValidator<UpdateCheckInStatus>
{
    public UpdateCheckInStatusValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
