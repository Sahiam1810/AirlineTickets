using FluentValidation;

namespace Application.UseCase.Staff;

public sealed class CreateStaffValidator : AbstractValidator<CreateStaff>
{
    public CreateStaffValidator()
    {
        RuleFor(x => x.PersonId)
            .NotEmpty().WithMessage("Person id is required.");

        RuleFor(x => x.StaffRoleId)
            .NotEmpty().WithMessage("Staff role id is required.");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date is required.");
    }
}
