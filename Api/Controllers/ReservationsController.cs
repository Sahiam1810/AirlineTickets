using Api.Dtos.Reservations;
using Application.Abstractions;
using Application.UseCase.Reservations;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers;

public sealed class ReservationsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ReservationsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReservationDto>>> GetAll(CancellationToken ct)
    {
        var reservations = await _uow.Reservations.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<ReservationDto>>(reservations);
        return Ok(result);
    }

    [HttpGet("paged")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        var reservations = await _uow.Reservations.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Reservations.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<ReservationDto>>(reservations);

        return Ok(new
        {
            page,
            pageSize,
            total,
            items
        });
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count([FromQuery] string? search = null, CancellationToken ct = default)
    {
        var total = await _uow.Reservations.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> GetById(int id, CancellationToken ct)
    {
        var reservation = await _uow.Reservations.GetByIdAsync(id, ct);
        if (reservation is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationDto>(reservation);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateReservation>(request);
        var id = await _sender.Send(command, ct);
        var reservation = await _uow.Reservations.GetByIdAsync(id, ct);
        if (reservation is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationDto>(reservation);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReservationRequest request, CancellationToken ct)
    {
        var existing = await _uow.Reservations.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateReservation>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var reservation = await _uow.Reservations.GetByIdAsync(id, ct);
        if (reservation is null)
        {
            return NotFound();
        }

        await _uow.Reservations.RemoveAsync(reservation, ct);
        await _uow.SaveChangesAsync(ct);

        return NoContent();
    }
}
