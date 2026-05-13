using MediatR;

namespace Application.UseCase.InvoiceItems;

public sealed record UpdateInvoiceItem(
    int Id,
    int InvoiceId,
    int InvoiceItemTypeId,
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal,
    int? ReservationPassengerId) : IRequest;
