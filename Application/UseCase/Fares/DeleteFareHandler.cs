using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Fares;

public sealed class DeleteFareHandler(IUnitOfWork uow) : IRequestHandler<DeleteFare>
{
    public async Task Handle(DeleteFare request, CancellationToken ct)
    {
        var fare = await uow.Fares.GetByIdAsync(request.Id, ct);

        if (fare is null)
            throw new Exception($"Fare with id {request.Id} not found.");

        await uow.Fares.RemoveAsync(fare, ct);
        await uow.SaveChangesAsync(ct);
    }
}
