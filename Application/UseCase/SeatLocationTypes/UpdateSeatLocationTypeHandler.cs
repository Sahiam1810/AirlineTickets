using Application.Abstractions;
using Domain.ValueObjects.SeatLocationTypes;
using MediatR;

namespace Application.UseCase.SeatLocationTypes;

public sealed class UpdateSeatLocationTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdateSeatLocationType>
{
    public async Task Handle(UpdateSeatLocationType request, CancellationToken ct)
    {
        var seatLocationType = await uow.SeatLocationTypes.GetByIdAsync(request.Id, ct);

        if (seatLocationType is null)
            throw new Exception($"SeatLocationType with id {request.Id} not found.");

        var name = SeatLocationTypeName.Create(request.Name);
        var sameName = string.Equals(seatLocationType.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.SeatLocationTypes.ExistsByNameAsync(name, request.Id, ct))
            throw new Exception($"SeatLocationType with name {name.Value} already exists.");

        seatLocationType.Update(request.Name);

        await uow.SeatLocationTypes.UpdateAsync(seatLocationType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
