using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed class MarkSeatOccupiedHandler(IUnitOfWork uow) : IRequestHandler<MarkSeatOccupied>
{
    public async Task Handle(MarkSeatOccupied request, CancellationToken ct)
    {
        var flightSeat = await uow.FlightSeats.GetByIdAsync(request.Id, ct);

        if (flightSeat is null)
            throw new Exception($"FlightSeat with id {request.Id} not found.");

        flightSeat.MarkAsOccupied();

        await uow.FlightSeats.UpdateAsync(flightSeat, ct);
        await uow.SaveChangesAsync(ct);
    }
}
