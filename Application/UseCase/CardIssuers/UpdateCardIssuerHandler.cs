using System;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed class UpdateCardIssuerHandler(IUnitOfWork uow) : IRequestHandler<UpdateCardIssuer>
{
    public async Task Handle(UpdateCardIssuer request, CancellationToken ct)
    {
        var cardIssuer = await uow.CardIssuers.GetByIdAsync(request.Id, ct);

        if (cardIssuer is null)
            throw new KeyNotFoundException($"Card issuer with id {request.Id} was not found.");

        cardIssuer.Update(
            request.Name
        );

        await uow.CardIssuers.UpdateAsync(cardIssuer, ct);
        await uow.SaveChangesAsync(ct);
    }
}
