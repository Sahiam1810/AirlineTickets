using Application.Abstractions;
using Domain.Entities.Routes;
using Domain.ValueObjects.Seasons;
using MediatR;

namespace Application.UseCase.Seasons;

public sealed class CreateSeasonHandler : IRequestHandler<CreateSeason, int>
{
    private readonly IUnitOfWork uow;

    public CreateSeasonHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<int> Handle(CreateSeason req, CancellationToken ct)
    {
        var name = SeasonName.Create(req.Name);
        _ = PriceFactor.Create(req.PriceFactor);

        if (await uow.Seasons.ExistsByNameAsync(name, ct))
            throw new Exception($"Season with name {name.Value} already exists.");

        var season = new Season(req.Name, req.Description, req.PriceFactor);

        await uow.Seasons.AddAsync(season, ct);
        await uow.SaveChangesAsync(ct);
        return season.Id;
    }
}
