using FluentValidation;

namespace Application.UseCase.StaffAvailabilities;

public sealed class CreateStaffAvailabilityValidator : AbstractValidator<CreateStaffAvailability>
{
    public CreateStaffAvailabilityValidator()
    {
        RuleFor(x => x.StaffId)
            .NotEmpty().WithMessage("Staff id is required.");

        RuleFor(x => x.AvailabilityStatusId)
            .NotEmpty().WithMessage("Availability status id is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date.");

        RuleFor(x => x.Notes)
            .MaximumLength(255);
    }
}
