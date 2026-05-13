using MediatR;

namespace Application.UseCase.ReservationStatusTransitions;

public sealed record CreateReservationStatusTransition(int FromStatusId, int ToStatusId) : IRequest<int>;
