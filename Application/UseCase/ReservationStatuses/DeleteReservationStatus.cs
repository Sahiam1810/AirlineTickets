using MediatR;

namespace Application.UseCase.ReservationStatuses;

public sealed record DeleteReservationStatus(int Id) : IRequest;
