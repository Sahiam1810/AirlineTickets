using FluentValidation;

namespace Application.UseCase.PaymentMethodTypes;

public sealed class UpdatePaymentMethodTypeValidator : AbstractValidator<UpdatePaymentMethodType>
{
    public UpdatePaymentMethodTypeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
