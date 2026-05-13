using MediatR;

namespace Application.UseCase.CheckInStatuses;

public sealed record CreateCheckInStatus(string Name) : IRequest<int>;
