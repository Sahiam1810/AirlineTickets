using Application.Abstractions;
using MediatR;

namespace Application.UseCase.ReservationPassengers;

public sealed class DeleteReservationPassengerHandler(IUnitOfWork uow) : IRequestHandler<DeleteReservationPassenger>
{
    public async Task Handle(DeleteReservationPassenger request, CancellationToken ct)
    {
        var reservationPassenger = await uow.ReservationPassengers.GetByIdAsync(request.Id, ct);

        if (reservationPassenger is null)
        {
            throw new KeyNotFoundException($"ReservationPassenger with id {request.Id} was not found.");
        }

        await uow.ReservationPassengers.RemoveAsync(reservationPassenger, ct);
        await uow.SaveChangesAsync(ct);
    }
}
