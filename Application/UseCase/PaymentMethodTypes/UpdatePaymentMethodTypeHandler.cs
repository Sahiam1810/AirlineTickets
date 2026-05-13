using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class UpdatePaymentMethodTypeHandler : IRequestHandler<UpdatePaymentMethodType>
{
    private readonly IUnitOfWork _uow;

    public UpdatePaymentMethodTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdatePaymentMethodType req, CancellationToken ct)
    {
        var paymentMethodType = await _uow.PaymentMethodTypes.GetByIdAsync(req.Id, ct);
        if (paymentMethodType is null)
        {
            throw new KeyNotFoundException($"Payment method type with id {req.Id} was not found.");
        }

        if (!string.Equals(paymentMethodType.Name, req.Name?.Trim(), StringComparison.OrdinalIgnoreCase)
            && await _uow.PaymentMethodTypes.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Payment method type with name '{req.Name}' already exists.");
        }

        paymentMethodType.Update(req.Name);

        await _uow.PaymentMethodTypes.UpdateAsync(paymentMethodType, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
