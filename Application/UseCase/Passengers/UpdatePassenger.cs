using MediatR;

namespace Application.UseCase.Passengers;

public sealed record UpdatePassenger(int Id, int PassengerTypeId) : IRequest;
