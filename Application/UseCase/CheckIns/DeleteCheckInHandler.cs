using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CheckIns;

public sealed class DeleteCheckInHandler(IUnitOfWork uow) : IRequestHandler<DeleteCheckIn>
{
    public async Task Handle(DeleteCheckIn request, CancellationToken ct)
    {
        var checkIn = await uow.CheckIns.GetByIdAsync(request.Id, ct);

        if (checkIn is null)
        {
            throw new KeyNotFoundException($"CheckIn with id {request.Id} was not found.");
        }

        await uow.CheckIns.RemoveAsync(checkIn, ct);
        await uow.SaveChangesAsync(ct);
    }
}
