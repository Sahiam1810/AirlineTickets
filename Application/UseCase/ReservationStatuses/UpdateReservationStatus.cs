using MediatR;

namespace Application.UseCase.ReservationStatuses;

public sealed record UpdateReservationStatus(int Id, string Name) : IRequest;
