using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Regions;

public sealed class DeleteRegionHandler(IUnitOfWork uow) : IRequestHandler<DeleteRegion>
{
    public async Task Handle(DeleteRegion request, CancellationToken ct)
    {
        var region = await uow.Regions.GetByIdAsync(request.Id, ct);

        if (region is null)
            throw new Exception($"Region with id {request.Id} not found.");

        await uow.Regions.RemoveAsync(region, ct);
        await uow.SaveChangesAsync(ct);
    }
}
