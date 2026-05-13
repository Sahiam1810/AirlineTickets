using Application.Abstractions;
using MediatR;

namespace Application.UseCase.ReservationStatusTransitions;

public sealed class DeleteReservationStatusTransitionHandler(IUnitOfWork uow) : IRequestHandler<DeleteReservationStatusTransition>
{
    public async Task Handle(DeleteReservationStatusTransition request, CancellationToken ct)
    {
        var reservationStatusTransition = await uow.ReservationStatusTransitions.GetByIdAsync(request.Id, ct);

        if (reservationStatusTransition is null)
            throw new Exception($"ReservationStatusTransition with id {request.Id} not found.");

        await uow.ReservationStatusTransitions.RemoveAsync(reservationStatusTransition, ct);
        await uow.SaveChangesAsync(ct);
    }
}
