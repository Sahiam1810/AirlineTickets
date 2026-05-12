using FluentValidation;

namespace Application.UseCase.Regions;

public sealed class CreateRegionValidator : AbstractValidator<CreateRegion>
{
    public CreateRegionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("El tipo es obligatorio.")
            .MaximumLength(30).WithMessage("El tipo no puede superar los 30 caracteres.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0).WithMessage("El pais es obligatorio.");
    }
}
