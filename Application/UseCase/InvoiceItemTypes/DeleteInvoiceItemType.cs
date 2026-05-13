using MediatR;

namespace Application.UseCase.InvoiceItemTypes;

public sealed record DeleteInvoiceItemType(int Id) : IRequest;
