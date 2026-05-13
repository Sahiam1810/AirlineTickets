using FluentValidation;

namespace Application.UseCase.SystemRoles;

public sealed class UpdateSystemRoleValidator : AbstractValidator<UpdateSystemRole>
{
    public UpdateSystemRoleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");
    }
}
