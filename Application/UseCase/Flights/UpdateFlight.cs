using MediatR;

namespace Application.UseCase.Flights;

public sealed record UpdateFlight(
    int Id,
    string FlightCode,
    int AirlineId,
    int RouteId,
    int AircraftId,
    DateTime DepartureDate,
    DateTime EstimatedArrivalDate,
    int TotalCapacity,
    int AvailableSeats,
    int FlightStateId,
    DateTime? RescheduledAt) : IRequest;
