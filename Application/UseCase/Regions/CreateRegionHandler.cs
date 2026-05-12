using Application.Abstractions;
using Domain.Entities.Geography;
using MediatR;

namespace Application.UseCase.Regions;

public sealed class CreateRegionHandler : IRequestHandler<CreateRegion, int>
{
    private readonly IUnitOfWork uow;

    public CreateRegionHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateRegion req, CancellationToken ct)
    {
        var country = await uow.Countries.GetByIdAsync(req.CountryId, ct);
        if (country is null)
            throw new Exception($"Country with id {req.CountryId} not found.");

        if (await uow.Regions.ExistsAsync(req.Name, req.CountryId, ct))
            throw new Exception($"Region with name {req.Name} already exists for country {req.CountryId}.");

        var region = new Region(req.Name, req.Type, req.CountryId);
        await uow.Regions.AddAsync(region, ct);
        await uow.SaveChangesAsync(ct);
        return region.Id;
    }
}
