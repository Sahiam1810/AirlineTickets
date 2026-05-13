using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Flights;

public sealed class DeleteFlightHandler(IUnitOfWork uow) : IRequestHandler<DeleteFlight>
{
    public async Task Handle(DeleteFlight request, CancellationToken ct)
    {
        var flight = await uow.Flights.GetByIdAsync(request.Id, ct);

        if (flight is null)
            throw new Exception($"Flight with id {request.Id} not found.");

        await uow.Flights.RemoveAsync(flight, ct);
        await uow.SaveChangesAsync(ct);
    }
}
