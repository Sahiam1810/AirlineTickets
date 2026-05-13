using MediatR;

namespace Application.UseCase.ReservationFlights;

public sealed record UpdateReservationFlight(
    int Id,
    int ReservationId,
    int FlightId,
    decimal PartialValue) : IRequest;
