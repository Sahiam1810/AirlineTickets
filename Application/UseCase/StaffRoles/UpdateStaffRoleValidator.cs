using FluentValidation;

namespace Application.UseCase.StaffRoles;

public sealed class UpdateStaffRoleValidator : AbstractValidator<UpdateStaffRole>
{
    public UpdateStaffRoleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Staff role name is required.")
            .MaximumLength(100);
    }
}
