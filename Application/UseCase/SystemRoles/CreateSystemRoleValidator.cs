using FluentValidation;

namespace Application.UseCase.SystemRoles;

public sealed class CreateSystemRoleValidator : AbstractValidator<CreateSystemRole>
{
    public CreateSystemRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.");
    }
}
