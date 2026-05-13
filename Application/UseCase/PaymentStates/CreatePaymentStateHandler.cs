using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.PaymentStates;

public sealed class CreatePaymentStateHandler : IRequestHandler<CreatePaymentState, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePaymentStateHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreatePaymentState req, CancellationToken ct)
    {
        if (await _uow.PaymentStates.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Payment state with name '{req.Name}' already exists.");
        }

        var paymentState = new PaymentState(req.Name);
        
        await _uow.PaymentStates.AddAsync(paymentState, ct);
        await _uow.SaveChangesAsync(ct);

        return paymentState.Id;
    }
}
