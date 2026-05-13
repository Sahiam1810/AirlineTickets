using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.Flights;
using MediatR;

namespace Application.UseCase.Flights;

public sealed class CreateFlightHandler : IRequestHandler<CreateFlight, int>
{
    private readonly IUnitOfWork uow;

    public CreateFlightHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateFlight req, CancellationToken ct)
    {
        var flightCode = FlightCode.Create(req.FlightCode);
        ValidateDates(req.DepartureDate, req.EstimatedArrivalDate);
        var capacity = Capacity.Create(req.TotalCapacity);
        _ = AvailableSeats.Create(req.AvailableSeats, capacity);

        await ValidateReferences(req.AirlineId, req.RouteId, req.AircraftId, req.FlightStateId, ct);

        if (await uow.Flights.ExistsByCodeAsync(flightCode, null, ct))
            throw new Exception($"Flight with code {flightCode.Value} already exists.");

        var flight = new Flight(
            req.FlightCode,
            req.AirlineId,
            req.RouteId,
            req.AircraftId,
            req.DepartureDate,
            req.EstimatedArrivalDate,
            req.TotalCapacity,
            req.AvailableSeats,
            req.FlightStateId,
            req.RescheduledAt);

        await uow.Flights.AddAsync(flight, ct);
        await uow.SaveChangesAsync(ct);
        return flight.Id;
    }

    private async Task ValidateReferences(int airlineId, int routeId, int aircraftId, int flightStateId, CancellationToken ct)
    {
        if (await uow.Airlines.GetByIdAsync(airlineId, ct) is null)
            throw new Exception($"Airline with id {airlineId} not found.");

        if (await uow.Routes.GetByIdAsync(routeId, ct) is null)
            throw new Exception($"Route with id {routeId} not found.");

        if (await uow.Aircraft.GetByIdAsync(aircraftId, ct) is null)
            throw new Exception($"Aircraft with id {aircraftId} not found.");

        if (await uow.FlightStates.GetByIdAsync(flightStateId, ct) is null)
            throw new Exception($"FlightState with id {flightStateId} not found.");
    }

    private static void ValidateDates(DateTime departureDate, DateTime estimatedArrivalDate)
    {
        if (departureDate >= estimatedArrivalDate)
            throw new Exception("DepartureDate must be earlier than EstimatedArrivalDate.");
    }
}
