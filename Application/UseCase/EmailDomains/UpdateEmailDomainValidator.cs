using FluentValidation;

namespace Application.UseCase.EmailDomains;

public sealed class UpdateEmailDomainValidator : AbstractValidator<UpdateEmailDomain>
{
    public UpdateEmailDomainValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");

        RuleFor(x => x.Domain)
            .NotEmpty().WithMessage("El dominio es obligatorio.")
            .MaximumLength(100);
    }
}
