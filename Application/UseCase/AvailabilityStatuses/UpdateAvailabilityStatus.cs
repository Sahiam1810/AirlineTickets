using MediatR;

namespace Application.UseCase.AvailabilityStatuses;

public sealed record UpdateAvailabilityStatus(int Id, string Name) : IRequest;
