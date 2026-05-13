using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed record CreateFlightSeat(
    int FlightId,
    string SeatCode,
    int CabinTypeId,
    int SeatLocationTypeId,
    bool IsOccupied) : IRequest<int>;
