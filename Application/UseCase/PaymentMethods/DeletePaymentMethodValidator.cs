using FluentValidation;

namespace Application.UseCase.PaymentMethods;

public sealed class DeletePaymentMethodValidator : AbstractValidator<DeletePaymentMethod>
{
    public DeletePaymentMethodValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}
