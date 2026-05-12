using Application.Abstractions;
using Domain.Entities.Location;
using MediatR;

namespace Application.UseCase.Addresses;

public sealed class CreateAddressHandler : IRequestHandler<CreateAddress, int>
{
    private readonly IUnitOfWork uow;

    public CreateAddressHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAddress req, CancellationToken ct)
    {
        var roadType = await uow.RoadTypes.GetByIdAsync(req.RoadTypeId, ct);
        if (roadType is null)
            throw new Exception($"RoadType with id {req.RoadTypeId} not found.");

        var city = await uow.Cities.GetByIdAsync(req.CityId, ct);
        if (city is null)
            throw new Exception($"City with id {req.CityId} not found.");

        var address = new Address(
            req.RoadTypeId,
            req.StreetName,
            req.Number,
            req.Complement,
            req.CityId,
            req.PostalCode);

        await uow.Addresses.AddAsync(address, ct);
        await uow.SaveChangesAsync(ct);
        return address.Id;
    }
}
