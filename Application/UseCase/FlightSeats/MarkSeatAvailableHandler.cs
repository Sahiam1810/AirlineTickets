using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed class MarkSeatAvailableHandler(IUnitOfWork uow) : IRequestHandler<MarkSeatAvailable>
{
    public async Task Handle(MarkSeatAvailable request, CancellationToken ct)
    {
        var flightSeat = await uow.FlightSeats.GetByIdAsync(request.Id, ct);

        if (flightSeat is null)
            throw new Exception($"FlightSeat with id {request.Id} not found.");

        flightSeat.MarkAsAvailable();

        await uow.FlightSeats.UpdateAsync(flightSeat, ct);
        await uow.SaveChangesAsync(ct);
    }
}
