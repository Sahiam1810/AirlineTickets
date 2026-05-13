using MediatR;

namespace Application.UseCase.ReservationStatusTransitions;

public sealed record DeleteReservationStatusTransition(int Id) : IRequest;
