using Api.Dtos.FlightSeats;
using Application.Abstractions;
using Application.UseCase.FlightSeats;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class FlightSeatsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public FlightSeatsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FlightSeatDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FlightSeatDto>>> GetAll(CancellationToken ct)
    {
        var flightSeats = await _uow.FlightSeats.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<FlightSeatDto>>(flightSeats);
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

        var flightSeats = await _uow.FlightSeats.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.FlightSeats.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<FlightSeatDto>>(flightSeats);

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
        var total = await _uow.FlightSeats.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FlightSeatDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightSeatDto>> GetById(int id, CancellationToken ct)
    {
        var flightSeat = await _uow.FlightSeats.GetByIdAsync(id, ct);
        if (flightSeat is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FlightSeatDto>(flightSeat);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FlightSeatDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateFlightSeatRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateFlightSeat>(request);
        var id = await _sender.Send(command, ct);
        var flightSeat = await _uow.FlightSeats.GetByIdAsync(id, ct);
        if (flightSeat is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FlightSeatDto>(flightSeat);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightSeatRequest request, CancellationToken ct)
    {
        var existing = await _uow.FlightSeats.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateFlightSeat>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var flightSeat = await _uow.FlightSeats.GetByIdAsync(id, ct);
        if (flightSeat is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteFlightSeat(id), ct);

        return NoContent();
    }

    [HttpPatch("{id:int}/occupy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Occupy(int id, CancellationToken ct)
    {
        var existing = await _uow.FlightSeats.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        await _sender.Send(new MarkSeatOccupied(id), ct);

        return NoContent();
    }

    [HttpPatch("{id:int}/release")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Release(int id, CancellationToken ct)
    {
        var existing = await _uow.FlightSeats.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        await _sender.Send(new MarkSeatAvailable(id), ct);

        return NoContent();
    }
}
