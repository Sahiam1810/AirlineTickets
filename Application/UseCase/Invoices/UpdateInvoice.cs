using MediatR;

namespace Application.UseCase.Invoices;

public sealed record UpdateInvoice(
    int Id,
    int ReservationId,
    string InvoiceNumber,
    DateTime IssuedAt,
    decimal Subtotal,
    decimal Taxes,
    decimal Total) : IRequest;
