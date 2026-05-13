using FluentValidation;

namespace Application.UseCase.PaymentStates;

public sealed class CreatePaymentStateValidator : AbstractValidator<CreatePaymentState>
{
    public CreatePaymentStateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
