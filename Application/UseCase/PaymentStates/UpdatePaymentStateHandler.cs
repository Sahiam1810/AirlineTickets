using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentStates;

public sealed class UpdatePaymentStateHandler : IRequestHandler<UpdatePaymentState>
{
    private readonly IUnitOfWork _uow;

    public UpdatePaymentStateHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdatePaymentState req, CancellationToken ct)
    {
        var paymentState = await _uow.PaymentStates.GetByIdAsync(req.Id, ct);
        if (paymentState is null)
        {
            throw new KeyNotFoundException($"Payment state with id {req.Id} was not found.");
        }

        if (paymentState.Name != req.Name && await _uow.PaymentStates.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Payment state with name '{req.Name}' already exists.");
        }

        paymentState.Update(req.Name);

        await _uow.PaymentStates.UpdateAsync(paymentState, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
