using Application.Abstractions;
using MediatR;

namespace Application.UseCase.ReservationPassengers;

public sealed class UpdateReservationPassengerHandler(IUnitOfWork uow) : IRequestHandler<UpdateReservationPassenger>
{
    public async Task Handle(UpdateReservationPassenger request, CancellationToken ct)
    {
        var reservationPassenger = await uow.ReservationPassengers.GetByIdAsync(request.Id, ct);

        if (reservationPassenger is null)
        {
            throw new KeyNotFoundException($"ReservationPassenger with id {request.Id} was not found.");
        }

        if (await uow.ReservationFlights.GetByIdAsync(request.ReservationFlightId, ct) is null)
        {
            throw new KeyNotFoundException($"ReservationFlight with id {request.ReservationFlightId} was not found.");
        }

        if (await uow.Passengers.GetByIdAsync(request.PassengerId, ct) is null)
        {
            throw new KeyNotFoundException($"Passenger with id {request.PassengerId} was not found.");
        }

        if (await uow.ReservationPassengers.ExistsAsync(request.ReservationFlightId, request.PassengerId, request.Id, ct))
        {
            throw new InvalidOperationException("The passenger is already associated with this reservation flight.");
        }

        reservationPassenger.Update(
            request.ReservationFlightId,
            request.PassengerId);

        await uow.ReservationPassengers.UpdateAsync(reservationPassenger, ct);
        await uow.SaveChangesAsync(ct);
    }
}
