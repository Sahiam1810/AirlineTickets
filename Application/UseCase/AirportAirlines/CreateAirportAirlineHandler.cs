using Application.Abstractions;
using Domain.Entities.Airlines;
using Domain.ValueObjects.AirportAirlines;
using MediatR;

namespace Application.UseCase.AirportAirlines;

public sealed class CreateAirportAirlineHandler : IRequestHandler<CreateAirportAirline, int>
{
    private readonly IUnitOfWork uow;

    public CreateAirportAirlineHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAirportAirline req, CancellationToken ct)
    {
        var airport = await uow.Airports.GetByIdAsync(req.AirportId, ct);
        if (airport is null)
            throw new Exception($"Airport with id {req.AirportId} not found.");

        var airline = await uow.Airlines.GetByIdAsync(req.AirlineId, ct);
        if (airline is null)
            throw new Exception($"Airline with id {req.AirlineId} not found.");

        if (await uow.AirportAirlines.ExistsAsync(req.AirportId, req.AirlineId, ct))
            throw new Exception($"AirportAirline with airport id {req.AirportId} and airline id {req.AirlineId} already exists.");

        if (req.EndDate.HasValue && req.EndDate.Value < req.StartDate)
            throw new Exception("End date cannot be earlier than start date.");

        var terminal = Terminal.Create(req.Terminal);
        var airportAirline = new AirportAirline(
            req.AirportId,
            req.AirlineId,
            terminal,
            req.StartDate,
            req.EndDate,
            req.IsActive);

        await uow.AirportAirlines.AddAsync(airportAirline, ct);
        await uow.SaveChangesAsync(ct);
        return airportAirline.Id;
    }
}
