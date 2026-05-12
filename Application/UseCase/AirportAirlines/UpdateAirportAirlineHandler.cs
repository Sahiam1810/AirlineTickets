using Application.Abstractions;
using Domain.ValueObjects.AirportAirlines;
using MediatR;

namespace Application.UseCase.AirportAirlines;

public sealed class UpdateAirportAirlineHandler(IUnitOfWork uow) : IRequestHandler<UpdateAirportAirline>
{
    public async Task Handle(UpdateAirportAirline request, CancellationToken ct)
    {
        var airportAirline = await uow.AirportAirlines.GetByIdAsync(request.Id, ct);

        if (airportAirline is null)
            throw new Exception($"AirportAirline with id {request.Id} not found.");

        var airport = await uow.Airports.GetByIdAsync(airportAirline.AirportId, ct);
        if (airport is null)
            throw new Exception($"Airport with id {airportAirline.AirportId} not found.");

        var airline = await uow.Airlines.GetByIdAsync(airportAirline.AirlineId, ct);
        if (airline is null)
            throw new Exception($"Airline with id {airportAirline.AirlineId} not found.");

        if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
            throw new Exception("End date cannot be earlier than start date.");

        var terminal = Terminal.Create(request.Terminal);

        airportAirline.Update(
            terminal,
            request.StartDate,
            request.EndDate,
            request.IsActive);

        await uow.AirportAirlines.UpdateAsync(airportAirline, ct);
        await uow.SaveChangesAsync(ct);
    }
}
