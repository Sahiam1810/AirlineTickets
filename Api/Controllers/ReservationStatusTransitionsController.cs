using Api.Dtos.ReservationStatusTransitions;
using Application.Abstractions;
using Application.UseCase.ReservationStatusTransitions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class ReservationStatusTransitionsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ReservationStatusTransitionsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReservationStatusTransitionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReservationStatusTransitionDto>>> GetAll(CancellationToken ct)
    {
        var reservationStatusTransitions = await _uow.ReservationStatusTransitions.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<ReservationStatusTransitionDto>>(reservationStatusTransitions);
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

        var reservationStatusTransitions = await _uow.ReservationStatusTransitions.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.ReservationStatusTransitions.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<ReservationStatusTransitionDto>>(reservationStatusTransitions);

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
        var total = await _uow.ReservationStatusTransitions.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservationStatusTransitionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationStatusTransitionDto>> GetById(int id, CancellationToken ct)
    {
        var reservationStatusTransition = await _uow.ReservationStatusTransitions.GetByIdAsync(id, ct);
        if (reservationStatusTransition is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationStatusTransitionDto>(reservationStatusTransition);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReservationStatusTransitionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateReservationStatusTransitionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateReservationStatusTransition>(request);
        var id = await _sender.Send(command, ct);
        var reservationStatusTransition = await _uow.ReservationStatusTransitions.GetByIdAsync(id, ct);
        if (reservationStatusTransition is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ReservationStatusTransitionDto>(reservationStatusTransition);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var reservationStatusTransition = await _uow.ReservationStatusTransitions.GetByIdAsync(id, ct);
        if (reservationStatusTransition is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteReservationStatusTransition(id), ct);

        return NoContent();
    }
}
