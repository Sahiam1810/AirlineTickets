using MediatR;

namespace Application.UseCase.ReservationPassengers;

public sealed record UpdateReservationPassenger(
    int Id,
    int ReservationFlightId,
    int PassengerId) : IRequest;
