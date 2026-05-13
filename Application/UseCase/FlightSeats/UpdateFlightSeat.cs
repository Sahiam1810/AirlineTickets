using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed record UpdateFlightSeat(
    int Id,
    int FlightId,
    string SeatCode,
    int CabinTypeId,
    int SeatLocationTypeId,
    bool IsOccupied) : IRequest;
