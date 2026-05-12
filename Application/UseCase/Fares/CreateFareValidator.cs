using FluentValidation;

namespace Application.UseCase.Fares;

public sealed class CreateFareValidator : AbstractValidator<CreateFare>
{
    public CreateFareValidator()
    {
        RuleFor(x => x.RouteId)
            .GreaterThan(0).WithMessage("La ruta es obligatoria.");

        RuleFor(x => x.CabinTypeId)
            .GreaterThan(0).WithMessage("El tipo de cabina es obligatorio.");

        RuleFor(x => x.PassengerTypeId)
            .GreaterThan(0).WithMessage("El tipo de pasajero es obligatorio.");

        RuleFor(x => x.SeasonId)
            .GreaterThan(0).WithMessage("La temporada es obligatoria.");

        RuleFor(x => x.BasePrice)
            .GreaterThanOrEqualTo(0).WithMessage("El precio base no puede ser negativo.");

        RuleFor(x => x)
            .Must(x => !x.ValidFrom.HasValue || !x.ValidTo.HasValue || x.ValidFrom.Value <= x.ValidTo.Value)
            .WithMessage("La fecha de vigencia inicial no puede ser mayor que la fecha de vigencia final.");
    }
}
