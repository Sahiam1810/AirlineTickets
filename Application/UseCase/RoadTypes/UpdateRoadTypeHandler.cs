using Application.Abstractions;
using MediatR;

namespace Application.UseCase.RoadTypes;

public sealed class UpdateRoadTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdateRoadType>
{
    public async Task Handle(UpdateRoadType request, CancellationToken ct)
    {
        var roadType = await uow.RoadTypes.GetByIdAsync(request.Id, ct);

        if (roadType is null)
            throw new Exception($"RoadType with id {request.Id} not found.");

        var sameName = string.Equals(roadType.Name.Value, request.Name.Trim(), StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.RoadTypes.ExistsAsync(request.Name, ct))
            throw new Exception($"RoadType with name {request.Name} already exists.");

        roadType.Update(
            request.Name
        );

        await uow.RoadTypes.UpdateAsync(roadType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
