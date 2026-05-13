using Application.Abstractions;
using Domain.Entities.Payments;
using MediatR;

namespace Application.UseCase.Invoices;

public sealed class CreateInvoiceHandler : IRequestHandler<CreateInvoice, int>
{
    private readonly IUnitOfWork _uow;

    public CreateInvoiceHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<int> Handle(CreateInvoice req, CancellationToken ct)
    {
        if (await _uow.Reservations.GetByIdAsync(req.ReservationId, ct) is null)
        {
            throw new KeyNotFoundException($"Reservation with id {req.ReservationId} was not found.");
        }

        if (await _uow.Invoices.ExistsByReservationAsync(req.ReservationId, ct))
        {
            throw new InvalidOperationException($"Reservation with id {req.ReservationId} already has an invoice.");
        }

        if (await _uow.Invoices.ExistsAsync(req.InvoiceNumber, ct))
        {
            throw new InvalidOperationException($"Invoice with number '{req.InvoiceNumber}' already exists.");
        }

        var invoice = new Invoice(
            req.ReservationId,
            req.InvoiceNumber,
            req.IssuedAt,
            req.Subtotal,
            req.Taxes,
            req.Total);

        await _uow.Invoices.AddAsync(invoice, ct);
        await _uow.SaveChangesAsync(ct);

        return invoice.Id;
    }
}
