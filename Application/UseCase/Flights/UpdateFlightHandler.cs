using Application.Abstractions;
using Domain.ValueObjects.Flights;
using MediatR;

namespace Application.UseCase.Flights;

public sealed class UpdateFlightHandler(IUnitOfWork uow) : IRequestHandler<UpdateFlight>
{
    public async Task Handle(UpdateFlight request, CancellationToken ct)
    {
        var flight = await uow.Flights.GetByIdAsync(request.Id, ct);

        if (flight is null)
            throw new Exception($"Flight with id {request.Id} not found.");

        var flightCode = FlightCode.Create(request.FlightCode);
        ValidateDates(request.DepartureDate, request.EstimatedArrivalDate);
        var capacity = Capacity.Create(request.TotalCapacity);
        _ = AvailableSeats.Create(request.AvailableSeats, capacity);

        await ValidateReferences(request.AirlineId, request.RouteId, request.AircraftId, request.FlightStateId, ct);

        if (await uow.Flights.ExistsByCodeAsync(flightCode, request.Id, ct))
            throw new Exception($"Flight with code {flightCode.Value} already exists.");

        flight.Update(
            request.FlightCode,
            request.AirlineId,
            request.RouteId,
            request.AircraftId,
            request.DepartureDate,
            request.EstimatedArrivalDate,
            request.TotalCapacity,
            request.AvailableSeats,
            request.FlightStateId,
            request.RescheduledAt);

        await uow.Flights.UpdateAsync(flight, ct);
        await uow.SaveChangesAsync(ct);
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
