using FluentValidation;

namespace Application.UseCase.StaffRoles;

public sealed class CreateStaffRoleValidator : AbstractValidator<CreateStaffRole>
{
    public CreateStaffRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Staff role name is required.")
            .MaximumLength(100);
    }
}
