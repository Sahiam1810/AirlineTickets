using Application.Abstractions;
using MediatR;

namespace Application.UseCase.Staff;

public sealed class DeleteStaffHandler(IUnitOfWork uow) : IRequestHandler<DeleteStaff>
{
    public async Task Handle(DeleteStaff request, CancellationToken ct)
    {
        var staff = await uow.Staff.GetByIdAsync(request.Id, ct);

        if (staff is null)
            throw new Exception($"Staff with id {request.Id} not found.");

        await uow.Staff.RemoveAsync(staff, ct);
        await uow.SaveChangesAsync(ct);
    }
}
