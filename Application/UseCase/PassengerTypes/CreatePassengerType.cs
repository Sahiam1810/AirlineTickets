using MediatR;

namespace Application.UseCase.PassengerTypes;

public sealed record CreatePassengerType(string Name, int? AgeMin, int? AgeMax) : IRequest<int>;
