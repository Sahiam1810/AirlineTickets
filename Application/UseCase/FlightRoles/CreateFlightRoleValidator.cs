using FluentValidation;

namespace Application.UseCase.FlightRoles;

public sealed class CreateFlightRoleValidator : AbstractValidator<CreateFlightRole>
{
    public CreateFlightRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100);
    }
}
