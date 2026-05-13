using MediatR;

namespace Application.UseCase.Passengers;

public sealed record CreatePassenger(int PersonId, int PassengerTypeId) : IRequest<int>;
