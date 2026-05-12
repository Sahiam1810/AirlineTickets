using Application.Abstractions;
using Domain.Entities.Aircraft;
using Domain.ValueObjects.Aircraft;
using MediatR;

namespace Application.UseCase.CabinTypes;

public sealed class CreateCabinTypeHandler : IRequestHandler<CreateCabinType, int>
{
    private readonly IUnitOfWork uow;

    public CreateCabinTypeHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateCabinType req, CancellationToken ct)
    {
        var name = CabinTypeName.Create(req.Name);

        if (await uow.CabinTypes.ExistsByNameAsync(name, ct))
            throw new Exception($"CabinType with name {name.Value} already exists.");

        var cabinType = new CabinType(name);

        await uow.CabinTypes.AddAsync(cabinType, ct);
        await uow.SaveChangesAsync(ct);
        return cabinType.Id;
    }
}
