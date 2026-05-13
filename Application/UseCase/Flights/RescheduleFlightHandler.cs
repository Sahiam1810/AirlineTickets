using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Flights;

public sealed class RescheduleFlightHandler(IUnitOfWork uow) : IRequestHandler<RescheduleFlight>
{
    public async Task Handle(RescheduleFlight request, CancellationToken ct)
    {
        var flight = await uow.Flights.GetByIdAsync(request.Id, ct);

        if (flight is null)
            throw new Exception($"Flight with id {request.Id} not found.");

        flight.Reschedule(request.DepartureDate, request.EstimatedArrivalDate);

        await uow.Flights.UpdateAsync(flight, ct);
        await uow.SaveChangesAsync(ct);
    }
}
