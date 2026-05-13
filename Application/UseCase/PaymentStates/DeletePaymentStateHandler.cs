using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentStates;

public sealed class DeletePaymentStateHandler : IRequestHandler<DeletePaymentState>
{
    private readonly IUnitOfWork _uow;

    public DeletePaymentStateHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(DeletePaymentState req, CancellationToken ct)
    {
        var paymentState = await _uow.PaymentStates.GetByIdAsync(req.Id, ct);
        if (paymentState is null)
        {
            throw new KeyNotFoundException($"Payment state with id {req.Id} was not found.");
        }

        await _uow.PaymentStates.RemoveAsync(paymentState, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
