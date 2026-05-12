using Application.Abstractions;
using Domain.Entities.Location;
using MediatR;

namespace Application.UseCase.RoadTypes;

public sealed class CreateRoadTypeHandler : IRequestHandler<CreateRoadType, int>
{
    private readonly IUnitOfWork uow;

    public CreateRoadTypeHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateRoadType req, CancellationToken ct)
    {
        if (await uow.RoadTypes.ExistsAsync(req.Name, ct))
            throw new Exception($"RoadType with name {req.Name} already exists.");

        var roadType = new RoadType(req.Name);
        await uow.RoadTypes.AddAsync(roadType, ct);
        await uow.SaveChangesAsync(ct);
        return roadType.Id;
    }
}
