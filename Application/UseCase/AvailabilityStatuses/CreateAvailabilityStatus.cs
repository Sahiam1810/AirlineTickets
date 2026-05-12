using MediatR;

namespace Application.UseCase.AvailabilityStatuses;

public sealed record CreateAvailabilityStatus(string Name) : IRequest<int>;
