using FluentValidation;

namespace Application.UseCase.InvoiceItemTypes;

public sealed class UpdateInvoiceItemTypeValidator : AbstractValidator<UpdateInvoiceItemType>
{
    public UpdateInvoiceItemTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
