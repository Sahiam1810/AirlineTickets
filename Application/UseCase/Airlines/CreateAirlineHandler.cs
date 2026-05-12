using Application.Abstractions;
using Domain.Entities.Airlines;
using Domain.ValueObjects.Airlines;
using MediatR;

namespace Application.UseCase.Airlines;

public sealed class CreateAirlineHandler : IRequestHandler<CreateAirline, int>
{
    private readonly IUnitOfWork uow;

    public CreateAirlineHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateAirline req, CancellationToken ct)
    {
        var country = await uow.Countries.GetByIdAsync(req.CountryId, ct);
        if (country is null)
            throw new Exception($"Country with id {req.CountryId} not found.");

        var name = AirlineName.Create(req.Name);
        var iataCode = IataCode.Create(req.IataCode);

        if (await uow.Airlines.ExistsByIataCodeAsync(iataCode.Value, ct))
            throw new Exception($"Airline with IATA code {iataCode.Value} already exists.");

        if (await uow.Airlines.ExistsByNameAsync(name.Value, ct))
            throw new Exception($"Airline with name {name.Value} already exists.");

        var airline = new Airline(name, iataCode, req.CountryId, req.IsActive);

        await uow.Airlines.AddAsync(airline, ct);
        await uow.SaveChangesAsync(ct);
        return airline.Id;
    }
}
