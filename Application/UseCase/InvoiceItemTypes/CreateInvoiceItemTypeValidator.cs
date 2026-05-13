using FluentValidation;

namespace Application.UseCase.InvoiceItemTypes;

public sealed class CreateInvoiceItemTypeValidator : AbstractValidator<CreateInvoiceItemType>
{
    public CreateInvoiceItemTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
