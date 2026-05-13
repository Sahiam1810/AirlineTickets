using FluentValidation;

namespace Application.UseCase.PaymentMethods;

public sealed class CreatePaymentMethodValidator : AbstractValidator<CreatePaymentMethod>
{
    public CreatePaymentMethodValidator()
    {
        RuleFor(x => x.PaymentMethodTypeId)
            .GreaterThan(0);

        RuleFor(x => x.CardTypeId)
            .GreaterThan(0)
            .When(x => x.CardTypeId.HasValue);

        RuleFor(x => x.CardIssuerId)
            .GreaterThan(0)
            .When(x => x.CardIssuerId.HasValue);

        RuleFor(x => x.CommercialName)
            .NotEmpty()
            .MaximumLength(50);
    }
}
