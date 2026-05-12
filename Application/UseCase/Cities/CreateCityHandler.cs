using Application.Abstractions;
using Domain.Entities.Geography;
using MediatR;

namespace Application.UseCase.Cities;

public sealed class CreateCityHandler : IRequestHandler<CreateCity, int>
{
    private readonly IUnitOfWork uow;

    public CreateCityHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateCity req, CancellationToken ct)
    {
        var region = await uow.Regions.GetByIdAsync(req.RegionId, ct);
        if (region is null)
            throw new Exception($"Region with id {req.RegionId} not found.");

        if (await uow.Cities.ExistsAsync(req.Name, req.RegionId, ct))
            throw new Exception($"City with name {req.Name} already exists for region {req.RegionId}.");

        var city = new City(req.Name, req.RegionId);
        await uow.Cities.AddAsync(city, ct);
        await uow.SaveChangesAsync(ct);
        return city.Id;
    }
}
