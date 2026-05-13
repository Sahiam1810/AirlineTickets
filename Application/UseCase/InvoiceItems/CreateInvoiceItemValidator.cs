using FluentValidation;

namespace Application.UseCase.InvoiceItems;

public sealed class CreateInvoiceItemValidator : AbstractValidator<CreateInvoiceItem>
{
    public CreateInvoiceItemValidator()
    {
        RuleFor(x => x.InvoiceId).GreaterThan(0);
        RuleFor(x => x.InvoiceItemTypeId).GreaterThan(0);
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(1);
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Subtotal)
            .GreaterThanOrEqualTo(0)
            .Equal(x => x.Quantity * x.UnitPrice).WithMessage("Subtotal must be exactly Quantity * UnitPrice.");
    }
}
