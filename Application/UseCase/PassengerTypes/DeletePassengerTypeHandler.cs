using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PassengerTypes;

public sealed class DeletePassengerTypeHandler(IUnitOfWork uow) : IRequestHandler<DeletePassengerType>
{
    public async Task Handle(DeletePassengerType request, CancellationToken ct)
    {
        var passengerType = await uow.PassengerTypes.GetByIdAsync(request.Id, ct);

        if (passengerType is null)
            throw new Exception($"PassengerType with id {request.Id} not found.");

        await uow.PassengerTypes.RemoveAsync(passengerType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
