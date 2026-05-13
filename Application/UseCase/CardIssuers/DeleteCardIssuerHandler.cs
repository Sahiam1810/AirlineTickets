using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed class DeleteCardIssuerHandler : IRequestHandler<DeleteCardIssuer>
{
    private readonly IUnitOfWork _uow;

    public DeleteCardIssuerHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeleteCardIssuer req, CancellationToken ct)
    {
        var cardIssuer = await _uow.CardIssuers.GetByIdAsync(req.Id, ct);
        if (cardIssuer is null)
        {
            throw new KeyNotFoundException($"Card issuer with id {req.Id} was not found.");
        }

        await _uow.CardIssuers.RemoveAsync(cardIssuer, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
