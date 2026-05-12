using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Airlines;

public sealed class DeleteAirlineHandler(IUnitOfWork uow) : IRequestHandler<DeleteAirline>
{
    public async Task Handle(DeleteAirline request, CancellationToken ct)
    {
        var airline = await uow.Airlines.GetByIdAsync(request.Id, ct);

        if (airline is null)
            throw new Exception($"Airline with id {request.Id} not found.");

        await uow.Airlines.RemoveAsync(airline, ct);
        await uow.SaveChangesAsync(ct);
    }
}
