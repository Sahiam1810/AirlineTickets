using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CardTypes;

public sealed class UpdateCardTypeHandler : IRequestHandler<UpdateCardType>
{
    private readonly IUnitOfWork _uow;

    public UpdateCardTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateCardType req, CancellationToken ct)
    {
        var cardType = await _uow.CardTypes.GetByIdAsync(req.Id, ct);
        if (cardType is null)
        {
            throw new KeyNotFoundException($"Card type with id {req.Id} was not found.");
        }

        if (!string.Equals(cardType.Name, req.Name?.Trim(), StringComparison.OrdinalIgnoreCase)
            && await _uow.CardTypes.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Card type with name '{req.Name}' already exists.");
        }

        cardType.Update(req.Name);

        await _uow.CardTypes.UpdateAsync(cardType, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
