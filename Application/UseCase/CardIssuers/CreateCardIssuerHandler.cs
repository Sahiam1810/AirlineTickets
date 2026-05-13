using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.CardIssuers;

public sealed class CreateCardIssuerHandler : IRequestHandler<CreateCardIssuer, int>
{
    private readonly IUnitOfWork _uow;

    public CreateCardIssuerHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateCardIssuer req, CancellationToken ct)
    {
        if (await _uow.CardIssuers.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Card issuer with name '{req.Name}' already exists.");
        }

        var cardIssuer = new CardIssuer(req.Name);

        await _uow.CardIssuers.AddAsync(cardIssuer, ct);
        await _uow.SaveChangesAsync(ct);

        return cardIssuer.Id;
    }
}
