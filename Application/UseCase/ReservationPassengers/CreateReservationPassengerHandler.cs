using Application.Abstractions;
using Domain.Entities.Reservations;
using MediatR;

namespace Application.UseCase.ReservationPassengers;

public sealed class CreateReservationPassengerHandler(IUnitOfWork uow) : IRequestHandler<CreateReservationPassenger, int>
{
    public async Task<int> Handle(CreateReservationPassenger request, CancellationToken ct)
    {
        if (await uow.ReservationFlights.GetByIdAsync(request.ReservationFlightId, ct) is null)
        {
            throw new KeyNotFoundException($"ReservationFlight with id {request.ReservationFlightId} was not found.");
        }

        if (await uow.Passengers.GetByIdAsync(request.PassengerId, ct) is null)
        {
            throw new KeyNotFoundException($"Passenger with id {request.PassengerId} was not found.");
        }

        if (await uow.ReservationPassengers.ExistsAsync(request.ReservationFlightId, request.PassengerId, ct: ct))
        {
            throw new InvalidOperationException("The passenger is already associated with this reservation flight.");
        }

        var reservationPassenger = new ReservationPassenger(
            request.ReservationFlightId,
            request.PassengerId);

        await uow.ReservationPassengers.AddAsync(reservationPassenger, ct);
        await uow.SaveChangesAsync(ct);

        return reservationPassenger.Id;
    }
}
