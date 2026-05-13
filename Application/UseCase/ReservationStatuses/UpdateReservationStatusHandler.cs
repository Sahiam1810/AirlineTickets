using Application.Abstractions;
using Domain.ValueObjects.ReservationStatuses;
using MediatR;

namespace Application.UseCase.ReservationStatuses;

public sealed class UpdateReservationStatusHandler(IUnitOfWork uow) : IRequestHandler<UpdateReservationStatus>
{
    public async Task Handle(UpdateReservationStatus request, CancellationToken ct)
    {
        var reservationStatus = await uow.ReservationStatuses.GetByIdAsync(request.Id, ct);

        if (reservationStatus is null)
            throw new Exception($"ReservationStatus with id {request.Id} not found.");

        var name = ReservationStatusName.Create(request.Name);
        var sameName = string.Equals(reservationStatus.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.ReservationStatuses.ExistsByNameAsync(name, request.Id, ct))
            throw new Exception($"ReservationStatus with name {name.Value} already exists.");

        reservationStatus.Update(request.Name);

        await uow.ReservationStatuses.UpdateAsync(reservationStatus, ct);
        await uow.SaveChangesAsync(ct);
    }
}
