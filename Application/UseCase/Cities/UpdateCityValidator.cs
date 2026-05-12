using FluentValidation;

namespace Application.UseCase.Cities;

public sealed class UpdateCityValidator : AbstractValidator<UpdateCity>
{
    public UpdateCityValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

        RuleFor(x => x.RegionId)
            .GreaterThan(0).WithMessage("La region es obligatoria.");
    }
}
