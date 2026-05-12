using FluentValidation;

namespace Application.UseCase.AvailabilityStatuses;

public sealed class UpdateAvailabilityStatusValidator : AbstractValidator<UpdateAvailabilityStatus>
{
    public UpdateAvailabilityStatusValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Availability status name is required.")
            .MaximumLength(50);
    }
}
