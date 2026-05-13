using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CardTypes;

public sealed class DeleteCardTypeHandler : IRequestHandler<DeleteCardType>
{
    private readonly IUnitOfWork _uow;

    public DeleteCardTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeleteCardType req, CancellationToken ct)
    {
        var cardType = await _uow.CardTypes.GetByIdAsync(req.Id, ct);
        if (cardType is null)
        {
            throw new KeyNotFoundException($"Card type with id {req.Id} was not found.");
        }

        await _uow.CardTypes.RemoveAsync(cardType, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
