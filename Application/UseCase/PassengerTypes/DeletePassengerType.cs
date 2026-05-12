using MediatR;

namespace Application.UseCase.PassengerTypes;

public sealed record DeletePassengerType(int Id) : IRequest;
