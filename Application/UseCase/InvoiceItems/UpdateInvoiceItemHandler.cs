using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using MediatR;

namespace Application.UseCase.InvoiceItems;

public sealed class UpdateInvoiceItemHandler : IRequestHandler<UpdateInvoiceItem>
{
    private readonly IUnitOfWork _uow;

    public UpdateInvoiceItemHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task Handle(UpdateInvoiceItem req, CancellationToken ct)
    {
        var invoiceItem = await _uow.InvoiceItems.GetByIdAsync(req.Id, ct);
        if (invoiceItem is null)
            throw new KeyNotFoundException($"Invoice item with id {req.Id} was not found.");

        if (invoiceItem.InvoiceId != req.InvoiceId && await _uow.Invoices.GetByIdAsync(req.InvoiceId, ct) is null)
            throw new ArgumentException($"Invoice with id {req.InvoiceId} does not exist.");

        if (invoiceItem.InvoiceItemTypeId != req.InvoiceItemTypeId
            && await _uow.InvoiceItemTypes.GetByIdAsync(req.InvoiceItemTypeId, ct) is null)
            throw new ArgumentException($"InvoiceItemType with id {req.InvoiceItemTypeId} does not exist.");

        if (req.ReservationPassengerId.HasValue
            && invoiceItem.ReservationPassengerId != req.ReservationPassengerId
            && await _uow.ReservationPassengers.GetByIdAsync(req.ReservationPassengerId.Value, ct) is null)
            throw new ArgumentException($"ReservationPassenger with id {req.ReservationPassengerId.Value} does not exist.");

        if ((invoiceItem.InvoiceId != req.InvoiceId || invoiceItem.Description != req.Description) && await _uow.InvoiceItems.ExistsAsync(req.InvoiceId, req.Description, ct))
            throw new InvalidOperationException($"An invoice item with description '{req.Description}' already exists for invoice {req.InvoiceId}.");

        invoiceItem.Update(
            req.InvoiceId,
            req.InvoiceItemTypeId,
            req.Description,
            req.Quantity,
            req.UnitPrice,
            req.Subtotal,
            req.ReservationPassengerId);

        await _uow.InvoiceItems.UpdateAsync(invoiceItem, ct);
        await _uow.SaveChangesAsync(ct);
    }
}
