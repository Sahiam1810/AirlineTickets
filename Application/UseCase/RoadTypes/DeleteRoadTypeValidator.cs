using FluentValidation;

namespace Application.UseCase.RoadTypes;

public sealed class DeleteRoadTypeValidator : AbstractValidator<DeleteRoadType>
{
    public DeleteRoadTypeValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El Id es obligatorio.");
    }
}
