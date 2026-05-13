using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.Payments;

public sealed class CreatePaymentHandler : IRequestHandler<CreatePayment, int>
{
    private readonly IUnitOfWork _uow;

    public CreatePaymentHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreatePayment req, CancellationToken ct)
    {
        await ValidateReferencesAsync(req.ReservationId, req.PaymentStateId, req.PaymentMethodId, ct);

        var payment = new Payment(
            req.ReservationId,
            req.Amount,
            req.PaymentDate,
            req.PaymentStateId,
            req.PaymentMethodId);

        await _uow.Payments.AddAsync(payment, ct);
        await _uow.SaveChangesAsync(ct);

        return payment.Id;
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
