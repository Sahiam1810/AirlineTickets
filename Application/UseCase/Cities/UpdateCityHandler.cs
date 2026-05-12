using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Cities;

public sealed class UpdateCityHandler(IUnitOfWork uow) : IRequestHandler<UpdateCity>
{
    public async Task Handle(UpdateCity request, CancellationToken ct)
    {
        var city = await uow.Cities.GetByIdAsync(request.Id, ct);

        if (city is null)
            throw new Exception($"City with id {request.Id} not found.");

        var region = await uow.Regions.GetByIdAsync(request.RegionId, ct);
        if (region is null)
            throw new Exception($"Region with id {request.RegionId} not found.");

        var sameName = string.Equals(city.Name.Value, request.Name.Trim(), StringComparison.OrdinalIgnoreCase);
        var sameRegion = city.RegionId == request.RegionId;

        if ((!sameName || !sameRegion) && await uow.Cities.ExistsAsync(request.Name, request.RegionId, ct))
            throw new Exception($"City with name {request.Name} already exists for region {request.RegionId}.");

        city.Update(
            request.Name,
            request.RegionId
        );

        await uow.Cities.UpdateAsync(city, ct);
        await uow.SaveChangesAsync(ct);
    }
}
