using FluentValidation;

namespace Application.UseCase.Permissions;

public sealed class CreatePermissionValidator : AbstractValidator<CreatePermission>
{
    public CreatePermissionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");
    }
}
