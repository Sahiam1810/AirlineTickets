using Application.Abstractions;
using MediatR;

namespace Application.UseCase.StaffRoles;

public sealed class DeleteStaffRoleHandler(IUnitOfWork uow) : IRequestHandler<DeleteStaffRole>
{
    public async Task Handle(DeleteStaffRole request, CancellationToken ct)
    {
        var staffRole = await uow.StaffRoles.GetByIdAsync(request.Id, ct);

        if (staffRole is null)
            throw new Exception($"StaffRole with id {request.Id} not found.");

        await uow.StaffRoles.RemoveAsync(staffRole, ct);
        await uow.SaveChangesAsync(ct);
    }
}
