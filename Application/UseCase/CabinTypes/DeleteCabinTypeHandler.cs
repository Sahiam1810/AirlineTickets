using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CabinTypes;

public sealed class DeleteCabinTypeHandler(IUnitOfWork uow) : IRequestHandler<DeleteCabinType>
{
    public async Task Handle(DeleteCabinType request, CancellationToken ct)
    {
        var cabinType = await uow.CabinTypes.GetByIdAsync(request.Id, ct);

        if (cabinType is null)
            throw new Exception($"CabinType with id {request.Id} not found.");

        await uow.CabinTypes.RemoveAsync(cabinType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
