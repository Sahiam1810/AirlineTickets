using FluentValidation;

namespace Application.UseCase.Staff;

public sealed class UpdateStaffValidator : AbstractValidator<UpdateStaff>
{
    public UpdateStaffValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.StaffRoleId)
            .NotEmpty().WithMessage("Staff role id is required.");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("Hire date is required.");
    }
}
