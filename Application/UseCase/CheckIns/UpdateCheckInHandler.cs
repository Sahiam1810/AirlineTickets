using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CheckIns;

public sealed class UpdateCheckInHandler(IUnitOfWork uow) : IRequestHandler<UpdateCheckIn>
{
    public async Task Handle(UpdateCheckIn request, CancellationToken ct)
    {
        var checkIn = await uow.CheckIns.GetByIdAsync(request.Id, ct);

        if (checkIn is null)
        {
            throw new KeyNotFoundException($"CheckIn with id {request.Id} was not found.");
        }

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

        if (await uow.CheckIns.ExistsAsync(boardingPassNumber, request.Id, ct))
        {
            throw new InvalidOperationException($"CheckIn with boarding pass number {boardingPassNumber} already exists.");
        }

        if (await uow.CheckIns.ExistsByTicketAsync(request.TicketId, request.Id, ct))
        {
            throw new InvalidOperationException("The ticket already has a check-in.");
        }

        if (await uow.CheckIns.ExistsByFlightSeatAsync(request.FlightSeatId, request.Id, ct))
        {
            throw new InvalidOperationException("The flight seat is already assigned to another check-in.");
        }

        checkIn.Update(
            request.TicketId,
            request.StaffId,
            request.FlightSeatId,
            request.CheckInDate,
            request.CheckInStatusId,
            boardingPassNumber,
            request.HasCheckedBaggage,
            request.CheckedBaggageWeightKg);

        await uow.CheckIns.UpdateAsync(checkIn, ct);
        await uow.SaveChangesAsync(ct);
    }
}
