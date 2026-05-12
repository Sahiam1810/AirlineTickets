using Application.Abstractions;
using MediatR;

namespace Application.UseCase.RoadTypes;

public sealed class DeleteRoadTypeHandler(IUnitOfWork uow) : IRequestHandler<DeleteRoadType>
{
    public async Task Handle(DeleteRoadType request, CancellationToken ct)
    {
        var roadType = await uow.RoadTypes.GetByIdAsync(request.Id, ct);

        if (roadType is null)
            throw new Exception($"RoadType with id {request.Id} not found.");

        await uow.RoadTypes.RemoveAsync(roadType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
