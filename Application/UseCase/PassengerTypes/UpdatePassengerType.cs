using MediatR;

namespace Application.UseCase.PassengerTypes;

public sealed record UpdatePassengerType(int Id, string Name, int? AgeMin, int? AgeMax) : IRequest;
