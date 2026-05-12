using FluentValidation;

namespace Application.UseCase.EmailDomains;

public sealed class CreateEmailDomainValidator : AbstractValidator<CreateEmailDomain>
{
    public CreateEmailDomainValidator()
    {
        RuleFor(x => x.Domain)
            .NotEmpty().WithMessage("El dominio es obligatorio.")
            .MaximumLength(100);
    }
}
