using Application.Abstractions;
using MediatR;

namespace Application.UseCase.AirportAirlines;

public sealed class DeleteAirportAirlineHandler(IUnitOfWork uow) : IRequestHandler<DeleteAirportAirline>
{
    public async Task Handle(DeleteAirportAirline request, CancellationToken ct)
    {
        var airportAirline = await uow.AirportAirlines.GetByIdAsync(request.Id, ct);

        if (airportAirline is null)
            throw new Exception($"AirportAirline with id {request.Id} not found.");

        await uow.AirportAirlines.RemoveAsync(airportAirline, ct);
        await uow.SaveChangesAsync(ct);
    }
}
