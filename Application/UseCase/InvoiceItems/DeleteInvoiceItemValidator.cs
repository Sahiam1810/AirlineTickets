using FluentValidation;

namespace Application.UseCase.InvoiceItems;

public sealed class DeleteInvoiceItemValidator : AbstractValidator<DeleteInvoiceItem>
{
    public DeleteInvoiceItemValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
