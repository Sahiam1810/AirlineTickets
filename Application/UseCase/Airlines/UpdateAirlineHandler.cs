using Application.Abstractions;
using Domain.ValueObjects.Airlines;
using MediatR;

namespace Application.UseCase.Airlines;

public sealed class UpdateAirlineHandler(IUnitOfWork uow) : IRequestHandler<UpdateAirline>
{
    public async Task Handle(UpdateAirline request, CancellationToken ct)
    {
        var airline = await uow.Airlines.GetByIdAsync(request.Id, ct);

        if (airline is null)
            throw new Exception($"Airline with id {request.Id} not found.");

        var country = await uow.Countries.GetByIdAsync(request.CountryId, ct);
        if (country is null)
            throw new Exception($"Country with id {request.CountryId} not found.");

        var name = AirlineName.Create(request.Name);
        var iataCode = IataCode.Create(request.IataCode);

        var sameCode = string.Equals(airline.IataCode.Value, iataCode.Value, StringComparison.OrdinalIgnoreCase);
        var sameName = string.Equals(airline.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameCode && await uow.Airlines.ExistsByIataCodeAsync(iataCode.Value, ct))
            throw new Exception($"Airline with IATA code {iataCode.Value} already exists.");

        if (!sameName && await uow.Airlines.ExistsByNameAsync(name.Value, ct))
            throw new Exception($"Airline with name {name.Value} already exists.");

        airline.Update(name, iataCode, request.CountryId, request.IsActive);

        await uow.Airlines.UpdateAsync(airline, ct);
        await uow.SaveChangesAsync(ct);
    }
}
