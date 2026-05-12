using Application.Abstractions;
using Domain.Entities.Geography;
using MediatR;

namespace Application.UseCase.Continents;

public sealed class CreateContinentHandler : IRequestHandler<CreateContinent, int>
{
    private readonly IUnitOfWork uow;

    public CreateContinentHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateContinent req, CancellationToken ct)
    {
        var continent = new Continent(req.Name);
        await uow.Continents.AddAsync(continent, ct);
        await uow.SaveChangesAsync(ct);
        return continent.Id;
    }
}