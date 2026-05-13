using MediatR;

namespace Application.UseCase.ReservationPassengers;

public sealed record CreateReservationPassenger(
    int ReservationFlightId,
    int PassengerId) : IRequest<int>;
