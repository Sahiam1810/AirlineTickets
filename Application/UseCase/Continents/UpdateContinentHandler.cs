using System;
using Application.Abstractions;
using Domain.Entities.Geography;
using MediatR;

namespace Application.UseCase.Continents;

public sealed class UpdateContinentHandler(IUnitOfWork uow) : IRequestHandler<UpdateContinent>
{
    public async Task Handle(UpdateContinent request, CancellationToken ct)
    {
        var continent = await uow.Continents.GetByIdAsync(request.Id, ct);

        if (continent is null)
            throw new Exception($"Continent with id {request.Id} not found.");

        continent.Updated(
            request.Name
        );

        await uow.Continents.UpdateAsync(continent, ct);
        await uow.SaveChangesAsync(ct);
    }
}