using FluentValidation;

namespace Application.UseCase.RoadTypes;

public sealed class UpdateRoadTypeValidator : AbstractValidator<UpdateRoadType>
{
    public UpdateRoadTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(50).WithMessage("El nombre no puede superar los 50 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");
    }
}
