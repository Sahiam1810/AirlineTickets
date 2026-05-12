using Application.Abstractions;
using Domain.Entities.Geography;
using MediatR;

namespace Application.UseCase.Countries;

public sealed class CreateCountryHandler : IRequestHandler<CreateCountry, int>
{
    private readonly IUnitOfWork uow;

    public CreateCountryHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateCountry req, CancellationToken ct)
    {
        var continent = await uow.Continents.GetByIdAsync(req.ContinentId, ct);
        if (continent is null)
            throw new Exception($"Continent with id {req.ContinentId} not found.");

        if (await uow.Countries.ExistsByNameAsync(req.Name, ct))
            throw new Exception($"Country with name {req.Name} already exists.");

        var country = new Country(req.Name, req.ContinentId);
        await uow.Countries.AddAsync(country, ct);
        await uow.SaveChangesAsync(ct);
        return country.Id;
    }
}
