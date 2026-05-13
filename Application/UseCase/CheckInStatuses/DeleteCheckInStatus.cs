using MediatR;

namespace Application.UseCase.CheckInStatuses;

public sealed record DeleteCheckInStatus(int Id) : IRequest;
