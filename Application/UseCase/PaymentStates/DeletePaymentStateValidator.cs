using FluentValidation;

namespace Application.UseCase.PaymentStates;

public sealed class DeletePaymentStateValidator : AbstractValidator<DeletePaymentState>
{
    public DeletePaymentStateValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
