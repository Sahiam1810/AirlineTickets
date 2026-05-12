using FluentValidation;

namespace Application.UseCase.Countries;

public sealed class UpdateCountryValidator : AbstractValidator<UpdateCountry>
{
    public UpdateCountryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("El nombre solo puede contener letras y espacios.");

        RuleFor(x => x.ContinentId)
            .GreaterThan(0).WithMessage("El continente es obligatorio.");
    }
}
