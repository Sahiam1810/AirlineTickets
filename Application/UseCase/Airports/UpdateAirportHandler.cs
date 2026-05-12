using Application.Abstractions;
using Domain.ValueObjects.Airports;
using MediatR;

namespace Application.UseCase.Airports;

public sealed class UpdateAirportHandler(IUnitOfWork uow) : IRequestHandler<UpdateAirport>
{
    public async Task Handle(UpdateAirport request, CancellationToken ct)
    {
        var airport = await uow.Airports.GetByIdAsync(request.Id, ct);

        if (airport is null)
            throw new Exception($"Airport with id {request.Id} not found.");

        var city = await uow.Cities.GetByIdAsync(request.CityId, ct);
        if (city is null)
            throw new Exception($"City with id {request.CityId} not found.");

        var name = AirportName.Create(request.Name);
        var iataCode = IataCode.Create(request.IataCode);
        var icaoCode = IcaoCode.Create(request.IcaoCode);

        var sameIataCode = string.Equals(airport.IataCode.Value, iataCode.Value, StringComparison.OrdinalIgnoreCase);
        var sameIcaoCode = string.Equals(airport.IcaoCode?.Value, icaoCode?.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameIataCode && await uow.Airports.ExistsByIataCodeAsync(iataCode.Value, ct))
            throw new Exception($"Airport with IATA code {iataCode.Value} already exists.");

        if (icaoCode is not null && !sameIcaoCode && await uow.Airports.ExistsByIcaoCodeAsync(icaoCode.Value, ct))
            throw new Exception($"Airport with ICAO code {icaoCode.Value} already exists.");

        airport.Update(name, iataCode, icaoCode, request.CityId);

        await uow.Airports.UpdateAsync(airport, ct);
        await uow.SaveChangesAsync(ct);
    }
}
