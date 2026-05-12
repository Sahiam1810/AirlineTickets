using MediatR;

namespace Application.UseCase.AvailabilityStatuses;

public sealed record DeleteAvailabilityStatus(int Id) : IRequest;
