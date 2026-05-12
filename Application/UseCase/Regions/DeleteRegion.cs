using MediatR;

namespace Application.UseCase.Regions;

public sealed record DeleteRegion(int Id) : IRequest;
