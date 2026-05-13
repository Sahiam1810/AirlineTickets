using Application.Abstractions;
using MediatR;

namespace Application.UseCase.ReservationStatuses;

public sealed class DeleteReservationStatusHandler(IUnitOfWork uow) : IRequestHandler<DeleteReservationStatus>
{
    public async Task Handle(DeleteReservationStatus request, CancellationToken ct)
    {
        var reservationStatus = await uow.ReservationStatuses.GetByIdAsync(request.Id, ct);

        if (reservationStatus is null)
            throw new Exception($"ReservationStatus with id {request.Id} not found.");

        await uow.ReservationStatuses.RemoveAsync(reservationStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
