using Application.Abstractions;
using Domain.Entities.Airlines;
using Domain.ValueObjects.Airports;
using MediatR;

namespace Application.UseCase.Airports;

public sealed class CreateAirportHandler : IRequestHandler<CreateAirport, int>
{
    private readonly IUnitOfWork uow;

    public CreateAirportHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAirport req, CancellationToken ct)
    {
        var city = await uow.Cities.GetByIdAsync(req.CityId, ct);
        if (city is null)
            throw new Exception($"City with id {req.CityId} not found.");

        var name = AirportName.Create(req.Name);
        var iataCode = IataCode.Create(req.IataCode);
        var icaoCode = IcaoCode.Create(req.IcaoCode);

        if (await uow.Airports.ExistsByIataCodeAsync(iataCode.Value, ct))
            throw new Exception($"Airport with IATA code {iataCode.Value} already exists.");

        if (icaoCode is not null && await uow.Airports.ExistsByIcaoCodeAsync(icaoCode.Value, ct))
            throw new Exception($"Airport with ICAO code {icaoCode.Value} already exists.");

        var airport = new Airport(name, iataCode, icaoCode, req.CityId);

        await uow.Airports.AddAsync(airport, ct);
        await uow.SaveChangesAsync(ct);
        return airport.Id;
    }
}
