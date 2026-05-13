using FluentValidation;

namespace Application.UseCase.Invoices;

public sealed class UpdateInvoiceValidator : AbstractValidator<UpdateInvoice>
{
    public UpdateInvoiceValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.ReservationId)
            .GreaterThan(0);

        RuleFor(x => x.InvoiceNumber)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.IssuedAt)
            .NotEmpty();

        RuleFor(x => x.Subtotal)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Taxes)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Total)
            .GreaterThanOrEqualTo(0)
            .Equal(x => x.Subtotal + x.Taxes)
            .WithMessage("Total must be equal to subtotal plus taxes.");
    }
}
