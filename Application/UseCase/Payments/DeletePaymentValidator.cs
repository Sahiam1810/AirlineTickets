using FluentValidation;

namespace Application.UseCase.Payments;

public sealed class DeletePaymentValidator : AbstractValidator<DeletePayment>
{
    public DeletePaymentValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
