using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Countries;

public sealed class UpdateCountryHandler(IUnitOfWork uow) : IRequestHandler<UpdateCountry>
{
    public async Task Handle(UpdateCountry request, CancellationToken ct)
    {
        var country = await uow.Countries.GetByIdAsync(request.Id, ct);

        if (country is null)
            throw new Exception($"Country with id {request.Id} not found.");

        var continent = await uow.Continents.GetByIdAsync(request.ContinentId, ct);
        if (continent is null)
            throw new Exception($"Continent with id {request.ContinentId} not found.");

        country.Update(
            request.Name,
            request.ContinentId
        );

        await uow.Countries.UpdateAsync(country, ct);
        await uow.SaveChangesAsync(ct);
    }
}
