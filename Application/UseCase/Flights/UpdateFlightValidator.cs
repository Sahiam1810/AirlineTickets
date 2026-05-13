using FluentValidation;

namespace Application.UseCase.Flights;

public sealed class UpdateFlightValidator : AbstractValidator<UpdateFlight>
{
    public UpdateFlightValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("El Id es obligatorio.");
        RuleFor(x => x.FlightCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.AirlineId).GreaterThan(0);
        RuleFor(x => x.RouteId).GreaterThan(0);
        RuleFor(x => x.AircraftId).GreaterThan(0);
        RuleFor(x => x.FlightStateId).GreaterThan(0);
        RuleFor(x => x.TotalCapacity).GreaterThan(0);
        RuleFor(x => x.AvailableSeats).GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .Must(x => x.AvailableSeats <= x.TotalCapacity)
            .WithMessage("Los asientos disponibles no pueden superar la capacidad total.");
        RuleFor(x => x)
            .Must(x => x.DepartureDate < x.EstimatedArrivalDate)
            .WithMessage("La fecha de salida debe ser menor que la fecha estimada de llegada.");
    }
}
