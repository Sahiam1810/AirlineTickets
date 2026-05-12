using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Airports;

public sealed class DeleteAirportHandler(IUnitOfWork uow) : IRequestHandler<DeleteAirport>
{
    public async Task Handle(DeleteAirport request, CancellationToken ct)
    {
        var airport = await uow.Airports.GetByIdAsync(request.Id, ct);

        if (airport is null)
            throw new Exception($"Airport with id {request.Id} not found.");

        await uow.Airports.RemoveAsync(airport, ct);
        await uow.SaveChangesAsync(ct);
    }
}
