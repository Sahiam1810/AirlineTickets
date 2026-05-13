using FluentValidation;

namespace Application.UseCase.InvoiceItemTypes;

public sealed class DeleteInvoiceItemTypeValidator : AbstractValidator<DeleteInvoiceItemType>
{
    public DeleteInvoiceItemTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
