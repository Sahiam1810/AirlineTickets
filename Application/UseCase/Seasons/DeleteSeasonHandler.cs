using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Seasons;

public sealed class DeleteSeasonHandler(IUnitOfWork uow) : IRequestHandler<DeleteSeason>
{
    public async Task Handle(DeleteSeason request, CancellationToken ct)
    {
        var season = await uow.Seasons.GetByIdAsync(request.Id, ct);

        if (season is null)
            throw new Exception($"Season with id {request.Id} not found.");

        await uow.Seasons.RemoveAsync(season, ct);
        await uow.SaveChangesAsync(ct);
    }
}
