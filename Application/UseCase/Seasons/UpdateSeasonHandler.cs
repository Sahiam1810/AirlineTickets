using Application.Abstractions;
using Domain.ValueObjects.Seasons;
using MediatR;

namespace Application.UseCase.Seasons;

public sealed class UpdateSeasonHandler(IUnitOfWork uow) : IRequestHandler<UpdateSeason>
{
    public async Task Handle(UpdateSeason request, CancellationToken ct)
    {
        var season = await uow.Seasons.GetByIdAsync(request.Id, ct);

        if (season is null)
            throw new Exception($"Season with id {request.Id} not found.");

        var name = SeasonName.Create(request.Name);
        _ = PriceFactor.Create(request.PriceFactor);
        var sameName = string.Equals(season.Name.Value, name.Value, StringComparison.OrdinalIgnoreCase);

        if (!sameName && await uow.Seasons.ExistsByNameAsync(name, ct))
            throw new Exception($"Season with name {name.Value} already exists.");

        season.Update(request.Name, request.Description, request.PriceFactor);

        await uow.Seasons.UpdateAsync(season, ct);
        await uow.SaveChangesAsync(ct);
    }
}
