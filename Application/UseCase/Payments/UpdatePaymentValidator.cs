using FluentValidation;

namespace Application.UseCase.Payments;

public sealed class UpdatePaymentValidator : AbstractValidator<UpdatePayment>
{
    public UpdatePaymentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ReservationId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PaymentDate).NotEmpty();
        RuleFor(x => x.PaymentStateId).GreaterThan(0);
        RuleFor(x => x.PaymentMethodId).GreaterThan(0);
    }
}
