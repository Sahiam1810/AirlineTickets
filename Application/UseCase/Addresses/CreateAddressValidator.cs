using FluentValidation;

namespace Application.UseCase.Addresses;

public sealed class CreateAddressValidator : AbstractValidator<CreateAddress>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.RoadTypeId)
            .GreaterThan(0).WithMessage("El tipo de via es obligatorio.");

        RuleFor(x => x.StreetName)
            .NotEmpty().WithMessage("El nombre de la via es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre de la via no puede superar los 100 caracteres.");

        RuleFor(x => x.Number)
            .MaximumLength(20).WithMessage("El numero no puede superar los 20 caracteres.");

        RuleFor(x => x.Complement)
            .MaximumLength(100).WithMessage("El complemento no puede superar los 100 caracteres.");

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("La ciudad es obligatoria.");

        RuleFor(x => x.PostalCode)
            .MaximumLength(20).WithMessage("El codigo postal no puede superar los 20 caracteres.");
    }
}
