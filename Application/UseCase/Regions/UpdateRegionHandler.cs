using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Regions;

public sealed class UpdateRegionHandler(IUnitOfWork uow) : IRequestHandler<UpdateRegion>
{
    public async Task Handle(UpdateRegion request, CancellationToken ct)
    {
        var region = await uow.Regions.GetByIdAsync(request.Id, ct);

        if (region is null)
            throw new Exception($"Region with id {request.Id} not found.");

        var country = await uow.Countries.GetByIdAsync(request.CountryId, ct);
        if (country is null)
            throw new Exception($"Country with id {request.CountryId} not found.");

        var sameName = string.Equals(region.Name.Value, request.Name.Trim(), StringComparison.OrdinalIgnoreCase);
        var sameCountry = region.CountryId == request.CountryId;

        if ((!sameName || !sameCountry) && await uow.Regions.ExistsAsync(request.Name, request.CountryId, ct))
            throw new Exception($"Region with name {request.Name} already exists for country {request.CountryId}.");

        region.Update(
            request.Name,
            request.Type,
            request.CountryId
        );

        await uow.Regions.UpdateAsync(region, ct);
        await uow.SaveChangesAsync(ct);
    }
}
