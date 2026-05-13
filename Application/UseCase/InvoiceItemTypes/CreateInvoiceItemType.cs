using MediatR;

namespace Application.UseCase.InvoiceItemTypes;

public sealed record CreateInvoiceItemType(string Name) : IRequest<int>;
