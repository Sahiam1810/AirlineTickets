using FluentValidation;

namespace Application.UseCase.AvailabilityStatuses;

public sealed class CreateAvailabilityStatusValidator : AbstractValidator<CreateAvailabilityStatus>
{
    public CreateAvailabilityStatusValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Availability status name is required.")
            .MaximumLength(50);
    }
}
