using FluentValidation;

namespace Application.UseCase.Permissions;

public sealed class UpdatePermissionValidator : AbstractValidator<UpdatePermission>
{
    public UpdatePermissionValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");
    }
}
