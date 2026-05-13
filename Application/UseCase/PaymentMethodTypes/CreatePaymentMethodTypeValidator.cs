using FluentValidation;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class CreatePaymentMethodTypeValidator : AbstractValidator<CreatePaymentMethodType>
{
    public CreatePaymentMethodTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
