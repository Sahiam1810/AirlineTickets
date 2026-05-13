using FluentValidation;

namespace Application.UseCase.PaymentStates;

public sealed class UpdatePaymentStateValidator : AbstractValidator<UpdatePaymentState>
{
    public UpdatePaymentStateValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
