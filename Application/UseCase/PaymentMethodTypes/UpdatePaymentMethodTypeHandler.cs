using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class UpdatePaymentMethodTypeHandler(IUnitOfWork uow) : IRequestHandler<UpdatePaymentMethodType>
{
    public async Task Handle(UpdatePaymentMethodType request, CancellationToken ct)
    {
        var paymentMethodType = await uow.PaymentMethodTypes.GetByIdAsync(request.Id, ct);
        
        if (paymentMethodType is null)
        {
            throw new KeyNotFoundException($"Payment method type with id {request.Id} was not found.");
        }

        paymentMethodType.Update(
            request.Name
        );

        await uow.PaymentMethodTypes.UpdateAsync(paymentMethodType, ct);
        await uow.SaveChangesAsync(ct);
    }
}
