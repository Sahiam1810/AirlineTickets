using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.InvoiceItemTypes;

public sealed class UpdateInvoiceItemTypeHandler : IRequestHandler<UpdateInvoiceItemType>
{
    private readonly IUnitOfWork _uow;

    public UpdateInvoiceItemTypeHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateInvoiceItemType req, CancellationToken ct)
    {
        var invoiceItemType = await _uow.InvoiceItemTypes.GetByIdAsync(req.Id, ct);
        if (invoiceItemType is null)
        {
            throw new KeyNotFoundException($"Invoice item type with id {req.Id} was not found.");
        }

        if (invoiceItemType.Name.Value != req.Name && await _uow.InvoiceItemTypes.ExistsAsync(req.Name, ct))
        {
            throw new InvalidOperationException($"Invoice item type with name '{req.Name}' already exists.");
        }

        invoiceItemType.Update(req.Name);

        await _uow.InvoiceItemTypes.UpdateAsync(invoiceItemType, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
