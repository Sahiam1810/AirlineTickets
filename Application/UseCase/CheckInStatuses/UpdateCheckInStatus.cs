using MediatR;

namespace Application.UseCase.CheckInStatuses;

public sealed record UpdateCheckInStatus(int Id, string Name) : IRequest;
