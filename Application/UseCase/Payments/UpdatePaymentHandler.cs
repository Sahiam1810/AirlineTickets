using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Payments;

public sealed class UpdatePaymentHandler : IRequestHandler<UpdatePayment>
{
    private readonly IUnitOfWork _uow;

    public UpdatePaymentHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdatePayment req, CancellationToken ct)
    {
        var payment = await _uow.Payments.GetByIdAsync(req.Id, ct);
        if (payment is null)
        {
            throw new KeyNotFoundException($"Payment with id {req.Id} was not found.");
        }

        await ValidateReferencesAsync(req.ReservationId, req.PaymentStateId, req.PaymentMethodId, ct);

        payment.Update(
            req.ReservationId,
            req.Amount,
            req.PaymentDate,
            req.PaymentStateId,
            req.PaymentMethodId);

        await _uow.Payments.UpdateAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);
    }

    private async Task ValidateReferencesAsync(int reservationId, int paymentStateId, int paymentMethodId, CancellationToken ct)
    {
        if (await _uow.Reservations.GetByIdAsync(reservationId, ct) is null)
        {
            throw new InvalidOperationException($"Reservation with id {reservationId} was not found.");
        }

        if (await _uow.PaymentStates.GetByIdAsync(paymentStateId, ct) is null)
        {
            throw new InvalidOperationException($"Payment state with id {paymentStateId} was not found.");
        }

        if (await _uow.PaymentMethods.GetByIdAsync(paymentMethodId, ct) is null)
        {
            throw new InvalidOperationException($"Payment method with id {paymentMethodId} was not found.");
        }
    }
}
