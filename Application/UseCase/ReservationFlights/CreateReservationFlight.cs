using MediatR;

namespace Application.UseCase.ReservationFlights;

public sealed record CreateReservationFlight(
    int ReservationId,
    int FlightId,
    decimal PartialValue) : IRequest<int>;
