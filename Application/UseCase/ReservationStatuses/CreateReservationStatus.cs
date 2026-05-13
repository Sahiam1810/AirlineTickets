using MediatR;

namespace Application.UseCase.ReservationStatuses;

public sealed record CreateReservationStatus(string Name) : IRequest<int>;
