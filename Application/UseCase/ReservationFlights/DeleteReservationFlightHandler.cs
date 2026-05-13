using Application.Abstractions;
using MediatR;

namespace Application.UseCase.ReservationFlights;

public sealed class DeleteReservationFlightHandler(IUnitOfWork uow) : IRequestHandler<DeleteReservationFlight>
{
    public async Task Handle(DeleteReservationFlight request, CancellationToken ct)
    {
        var reservationFlight = await uow.ReservationFlights.GetByIdAsync(request.Id, ct);

        if (reservationFlight is null)
        {
            throw new KeyNotFoundException($"ReservationFlight with id {request.Id} was not found.");
        }

        await uow.ReservationFlights.RemoveAsync(reservationFlight, ct);
        await uow.SaveChangesAsync(ct);
    }
}
