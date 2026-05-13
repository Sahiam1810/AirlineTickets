using MediatR;

namespace Application.UseCase.ReservationPassengers;

public sealed record DeleteReservationPassenger(int Id) : IRequest;
