using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Addresses;

public sealed class UpdateAddressHandler(IUnitOfWork uow) : IRequestHandler<UpdateAddress>
{
    public async Task Handle(UpdateAddress request, CancellationToken ct)
    {
        var address = await uow.Addresses.GetByIdAsync(request.Id, ct);

        if (address is null)
            throw new Exception($"Address with id {request.Id} not found.");

        var roadType = await uow.RoadTypes.GetByIdAsync(request.RoadTypeId, ct);
        if (roadType is null)
            throw new Exception($"RoadType with id {request.RoadTypeId} not found.");

        var city = await uow.Cities.GetByIdAsync(request.CityId, ct);
        if (city is null)
            throw new Exception($"City with id {request.CityId} not found.");

        address.Update(
            request.RoadTypeId,
            request.StreetName,
            request.Number,
            request.Complement,
            request.CityId,
            request.PostalCode
        );

        await uow.Addresses.UpdateAsync(address, ct);
        await uow.SaveChangesAsync(ct);
    }
}
