using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Context;

namespace Application.UseCase.InvoiceItems;

public sealed class CreateInvoiceItemHandler : IRequestHandler<CreateInvoiceItem, int>
{
    private readonly IUnitOfWork _uow;
    private readonly AppDbContext _context;

    public CreateInvoiceItemHandler(IUnitOfWork uow, AppDbContext context)
    {
        _uow = uow;
        _context = context;
    }

    public async Task<int> Handle(CreateInvoiceItem req, CancellationToken ct)
    {
        if (!await _context.Invoices.AnyAsync(x => x.Id == req.InvoiceId, ct))
            throw new ArgumentException($"Invoice with id {req.InvoiceId} does not exist.");

        if (!await _context.InvoiceItemTypes.AnyAsync(x => x.Id == req.InvoiceItemTypeId, ct))
            throw new ArgumentException($"InvoiceItemType with id {req.InvoiceItemTypeId} does not exist.");

        if (req.ReservationPassengerId.HasValue && !await _context.ReservationPassengers.AnyAsync(x => x.Id == req.ReservationPassengerId.Value, ct))
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
