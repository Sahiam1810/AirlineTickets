using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Countries;

public sealed class DeleteCountryHandler(IUnitOfWork uow) : IRequestHandler<DeleteCountry>
{
    public async Task Handle(DeleteCountry request, CancellationToken ct)
    {
        var country = await uow.Countries.GetByIdAsync(request.Id, ct);

        if (country is null)
            throw new Exception($"Country with id {request.Id} not found.");

        await uow.Countries.RemoveAsync(country, ct);
        await uow.SaveChangesAsync(ct);
    }
}
