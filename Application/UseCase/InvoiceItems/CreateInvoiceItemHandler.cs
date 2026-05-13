using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.InvoiceItems;

public sealed class CreateInvoiceItemHandler : IRequestHandler<CreateInvoiceItem, int>
{
    private readonly IUnitOfWork _uow;

    public CreateInvoiceItemHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateInvoiceItem req, CancellationToken ct)
    {
        if (await _uow.Invoices.GetByIdAsync(req.InvoiceId, ct) is null)
            throw new ArgumentException($"Invoice with id {req.InvoiceId} does not exist.");

        if (await _uow.InvoiceItemTypes.GetByIdAsync(req.InvoiceItemTypeId, ct) is null)
            throw new ArgumentException($"InvoiceItemType with id {req.InvoiceItemTypeId} does not exist.");

        if (req.ReservationPassengerId.HasValue
            && await _uow.ReservationPassengers.GetByIdAsync(req.ReservationPassengerId.Value, ct) is null)
            throw new ArgumentException($"ReservationPassenger with id {req.ReservationPassengerId.Value} does not exist.");

        if (await _uow.InvoiceItems.ExistsAsync(req.InvoiceId, req.Description, ct))
            throw new InvalidOperationException($"An invoice item with description '{req.Description}' already exists for invoice {req.InvoiceId}.");

        var invoiceItem = new InvoiceItem(
            req.InvoiceId,
            req.InvoiceItemTypeId,
            req.Description,
            req.Quantity,
            req.UnitPrice,
            req.Subtotal,
            req.ReservationPassengerId);

        await _uow.InvoiceItems.AddAsync(invoiceItem, ct);
        await _uow.SaveChangesAsync(ct);

        return invoiceItem.Id;
    }
}
