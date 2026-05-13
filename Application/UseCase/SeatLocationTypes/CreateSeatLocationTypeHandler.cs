using Application.Abstractions;
using Domain.Entities.Flights;
using Domain.ValueObjects.SeatLocationTypes;
using MediatR;

namespace Application.UseCase.SeatLocationTypes;

public sealed class CreateSeatLocationTypeHandler : IRequestHandler<CreateSeatLocationType, int>
{
    private readonly IUnitOfWork uow;

    public CreateSeatLocationTypeHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateSeatLocationType req, CancellationToken ct)
    {
        var name = SeatLocationTypeName.Create(req.Name);

        if (await uow.SeatLocationTypes.ExistsByNameAsync(name, null, ct))
            throw new Exception($"SeatLocationType with name {name.Value} already exists.");

        var seatLocationType = new SeatLocationType(req.Name);

        await uow.SeatLocationTypes.AddAsync(seatLocationType, ct);
        await uow.SaveChangesAsync(ct);
        return seatLocationType.Id;
    }
}
