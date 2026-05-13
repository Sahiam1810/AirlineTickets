using FluentValidation;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class DeletePaymentMethodTypeValidator : AbstractValidator<DeletePaymentMethodType>
{
    public DeletePaymentMethodTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
