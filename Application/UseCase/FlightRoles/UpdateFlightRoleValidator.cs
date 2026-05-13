using FluentValidation;

namespace Application.UseCase.FlightRoles;

public sealed class UpdateFlightRoleValidator : AbstractValidator<UpdateFlightRole>
{
    public UpdateFlightRoleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100);
    }
}
