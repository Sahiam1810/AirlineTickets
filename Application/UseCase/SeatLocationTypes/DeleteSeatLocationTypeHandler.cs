using Application.Abstractions;
using MediatR;

namespace Application.UseCase.SeatLocationTypes;

public sealed class DeleteSeatLocationTypeHandler(IUnitOfWork uow) : IRequestHandler<DeleteSeatLocationType>
{
    public async Task Handle(DeleteSeatLocationType request, CancellationToken ct)
    {
        var seatLocationType = await uow.SeatLocationTypes.GetByIdAsync(request.Id, ct);

        if (seatLocationType is null)
            throw new Exception($"SeatLocationType with id {request.Id} not found.");

        await uow.SeatLocationTypes.RemoveAsync(seatLocationType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
