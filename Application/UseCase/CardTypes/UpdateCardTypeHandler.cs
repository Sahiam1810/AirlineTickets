using System;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CardTypes;

public sealed class UpdateCardTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdateCardType>
{
    public async Task Handle(UpdateCardType request, CancellationToken ct)
    {
        var cardType = await uow.CardTypes.GetByIdAsync(request.Id, ct);
        
        if (cardType is null)
        {
            throw new KeyNotFoundException($"Card type with id {request.Id} was not found.");
        }

        cardType.Update(
            request.Name
        );

        await uow.CardTypes.UpdateAsync(cardType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
