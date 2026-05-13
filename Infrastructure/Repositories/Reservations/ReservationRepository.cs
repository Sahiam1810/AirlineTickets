using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Domain.Entities.Reservations;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Reservations;

public sealed class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Reservation?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(x => x.ReservationCode.Value == code, ct);
    }

    public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Reservations
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Reservation>> GetPagedAsync(int page, int pageSize, string? search, CancellationToken ct = default)
    {
        var query = _context.Reservations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.ReservationCode.Value, $"%{search}%"));
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(string? search = null, CancellationToken ct = default)
    {
        var query = _context.Reservations.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x => EF.Functions.ILike(x.ReservationCode.Value, $"%{search}%"));
        }

        return await query.CountAsync(ct);
    }

    public async Task AddAsync(Reservation reservation, CancellationToken ct = default)
    {
        await _context.Reservations.AddAsync(reservation, ct);
    }

    public Task UpdateAsync(Reservation reservation, CancellationToken ct = default)
    {
        _context.Reservations.Update(reservation);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Reservation reservation, CancellationToken ct = default)
    {
        _context.Reservations.Remove(reservation);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.Reservations
            .AnyAsync(x => x.ReservationCode.Value == code, ct);
    }
}
