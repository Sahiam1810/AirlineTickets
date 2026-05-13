using FluentValidation;

namespace Application.UseCase.Payments;

public sealed class CreatePaymentValidator : AbstractValidator<CreatePayment>
{
    public CreatePaymentValidator()
    {
        RuleFor(x => x.ReservationId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PaymentDate).NotEmpty();
        RuleFor(x => x.PaymentStateId).GreaterThan(0);
        RuleFor(x => x.PaymentMethodId).GreaterThan(0);
    }
}
