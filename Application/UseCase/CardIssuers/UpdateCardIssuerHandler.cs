using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed class UpdateCardIssuerHandler : IRequestHandler<UpdateCardIssuer>
{
    private readonly IUnitOfWork _uow;

    public UpdateCardIssuerHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateCardIssuer req, CancellationToken ct)
    {
        var cardIssuer = await _uow.CardIssuers.GetByIdAsync(req.Id, ct);
        if (cardIssuer is null)
        {
            throw new KeyNotFoundException($"Card issuer with id {req.Id} was not found.");
        }

        if (!string.Equals(cardIssuer.Name, req.Name?.Trim(), StringComparison.OrdinalIgnoreCase)
            && await _uow.CardIssuers.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Card issuer with name '{req.Name}' already exists.");
        }

        cardIssuer.Update(req.Name);

        await _uow.CardIssuers.UpdateAsync(cardIssuer, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
