using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Cities;

public sealed class DeleteCityHandler(IUnitOfWork uow) : IRequestHandler<DeleteCity>
{
    public async Task Handle(DeleteCity request, CancellationToken ct)
    {
        var city = await uow.Cities.GetByIdAsync(request.Id, ct);

        if (city is null)
            throw new Exception($"City with id {request.Id} not found.");

        await uow.Cities.RemoveAsync(city, ct);
        await uow.SaveChangesAsync(ct);
    }
}
