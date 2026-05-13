using Application.Abstractions;
using MediatR;

namespace Application.UseCase.FlightSeats;

public sealed class DeleteFlightSeatHandler(IUnitOfWork uow) : IRequestHandler<DeleteFlightSeat>
{
    public async Task Handle(DeleteFlightSeat request, CancellationToken ct)
    {
        var flightSeat = await uow.FlightSeats.GetByIdAsync(request.Id, ct);

        if (flightSeat is null)
            throw new Exception($"FlightSeat with id {request.Id} not found.");

        await uow.FlightSeats.RemoveAsync(flightSeat, ct);
        await uow.SaveChangesAsync(ct);
    }
}
