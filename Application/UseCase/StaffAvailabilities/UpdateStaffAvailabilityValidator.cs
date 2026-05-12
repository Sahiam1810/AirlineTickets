using FluentValidation;

namespace Application.UseCase.StaffAvailabilities;

public sealed class UpdateStaffAvailabilityValidator : AbstractValidator<UpdateStaffAvailability>
{
    public UpdateStaffAvailabilityValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.AvailabilityStatusId)
            .NotEmpty().WithMessage("Availability status id is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date.");

        RuleFor(x => x.Notes)
            .MaximumLength(255);
    }
}
