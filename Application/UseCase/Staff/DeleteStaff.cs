using MediatR;

namespace Application.UseCase.Staff;

public sealed record DeleteStaff(int Id) : IRequest;
