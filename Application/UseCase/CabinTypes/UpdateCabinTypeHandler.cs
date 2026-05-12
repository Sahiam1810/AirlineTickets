using Application.Abstractions;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.CabinTypes;

public sealed class UpdateCabinTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdateCabinType>
{
    public async Task Handle(UpdateCabinType request, CancellationToken ct)
    {
        var cabinType = await uow.CabinTypes.GetByIdAsync(request.Id, ct);

        if (cabinType is null)
            throw new Exception($"CabinType with id {request.Id} not found.");

        var name = CabinTypeName.Create(request.Name);
        var sameName = string.Equals(cabinType.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.CabinTypes.ExistsByNameAsync(name, ct))
            throw new Exception($"CabinType with name {name.Value} already exists.");

        cabinType.Update(name);

        await uow.CabinTypes.UpdateAsync(cabinType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
