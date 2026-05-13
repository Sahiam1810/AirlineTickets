using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentMethods;

public sealed class UpdatePaymentMethodHandler : IRequestHandler<UpdatePaymentMethod>
{
    private readonly IUnitOfWork _uow;

    public UpdatePaymentMethodHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdatePaymentMethod req, CancellationToken ct)
    {
        var paymentMethod = await _uow.PaymentMethods.GetByIdAsync(req.Id, ct);
        if (paymentMethod is null)
        {
            throw new KeyNotFoundException($"Payment method with id {req.Id} was not found.");
        }

        await ValidateReferencesAsync(req.PaymentMethodTypeId, req.CardTypeId, req.CardIssuerId, ct);

        if (!string.Equals(paymentMethod.CommercialName, req.CommercialName, StringComparison.OrdinalIgnoreCase)
            && await _uow.PaymentMethods.ExistsAsync(req.CommercialName, ct))
        {
            throw new InvalidOperationException($"Payment method with commercial name '{req.CommercialName}' already exists.");
        }

        paymentMethod.Update(
            req.PaymentMethodTypeId,
            req.CardTypeId,
            req.CardIssuerId,
            req.CommercialName);

        await _uow.PaymentMethods.UpdateAsync(paymentMethod, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private async Task ValidateReferencesAsync(int paymentMethodTypeId, int? cardTypeId, int? cardIssuerId, CancellationToken ct)
    {
        if (cardTypeId.HasValue != cardIssuerId.HasValue)
        {
            throw new InvalidOperationException("Card type and card issuer must be provided together.");
        }

        if (await _uow.PaymentMethodTypes.GetByIdAsync(paymentMethodTypeId, ct) is null)
        {
            throw new InvalidOperationException($"Payment method type with id {paymentMethodTypeId} was not found.");
        }

        if (cardTypeId.HasValue && await _uow.CardTypes.GetByIdAsync(cardTypeId.Value, ct) is null)
        {
            throw new InvalidOperationException($"Card type with id {cardTypeId.Value} was not found.");
        }

        if (cardIssuerId.HasValue && await _uow.CardIssuers.GetByIdAsync(cardIssuerId.Value, ct) is null)
        {
            throw new InvalidOperationException($"Card issuer with id {cardIssuerId.Value} was not found.");
        }
    }
}
