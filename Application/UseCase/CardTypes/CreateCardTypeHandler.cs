using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.CardTypes;

public sealed class CreateCardTypeHandler : IRequestHandler<CreateCardType, int>
{
    private readonly IUnitOfWork _uow;

    public CreateCardTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateCardType req, CancellationToken ct)
    {
        if (await _uow.CardTypes.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Card type with name '{req.Name}' already exists.");
        }

        var cardType = new CardType(req.Name);

        await _uow.CardTypes.AddAsync(cardType, ct);
        await _uow.SaveChangesAsync(ct);

        return cardType.Id;
    }
}
