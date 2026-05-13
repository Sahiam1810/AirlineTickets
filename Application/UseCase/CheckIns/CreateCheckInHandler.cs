using Application.Abstractions;
using Domain.Entities.Tickets;
using MediatR;

namespace Application.UseCase.CheckIns;

public sealed class CreateCheckInHandler(IUnitOfWork uow) : IRequestHandler<CreateCheckIn, int>
{
    public async Task<int> Handle(CreateCheckIn request, CancellationToken ct)
    {
        var boardingPassNumber = request.BoardingPassNumber.Trim();

        if (await uow.Tickets.GetByIdAsync(request.TicketId, ct) is null)
        {
            throw new KeyNotFoundException($"Ticket with id {request.TicketId} was not found.");
        }

        if (await uow.Staff.GetByIdAsync(request.StaffId, ct) is null)
        {
            throw new KeyNotFoundException($"Staff with id {request.StaffId} was not found.");
        }

        if (await uow.FlightSeats.GetByIdAsync(request.FlightSeatId, ct) is null)
        {
            throw new KeyNotFoundException($"FlightSeat with id {request.FlightSeatId} was not found.");
        }

        if (await uow.CheckInStatuses.GetByIdAsync(request.CheckInStatusId, ct) is null)
        {
            throw new KeyNotFoundException($"CheckInStatus with id {request.CheckInStatusId} was not found.");
        }

        if (await uow.CheckIns.ExistsAsync(boardingPassNumber, null, ct))
        {
            throw new InvalidOperationException($"CheckIn with boarding pass number {boardingPassNumber} already exists.");
        }

        if (await uow.CheckIns.ExistsByTicketAsync(request.TicketId, null, ct))
        {
            throw new InvalidOperationException("The ticket already has a check-in.");
        }

        if (await uow.CheckIns.ExistsByFlightSeatAsync(request.FlightSeatId, null, ct))
        {
            throw new InvalidOperationException("The flight seat is already assigned to another check-in.");
        }

        var checkIn = new CheckIn(
            request.TicketId,
            request.StaffId,
            request.FlightSeatId,
            request.CheckInDate,
            request.CheckInStatusId,
            boardingPassNumber,
            request.HasCheckedBaggage,
            request.CheckedBaggageWeightKg);

        await uow.CheckIns.AddAsync(checkIn, ct);
        await uow.SaveChangesAsync(ct);

        return checkIn.Id;
    }
}
