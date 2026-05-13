using Api.Dtos.FlightStatusTransitions;
using Application.Abstractions;
using Application.UseCase.FlightStatusTransitions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class FlightStatusTransitionsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public FlightStatusTransitionsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FlightStatusTransitionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FlightStatusTransitionDto>>> GetAll(CancellationToken ct)
    {
        var flightStatusTransitions = await _uow.FlightStatusTransitions.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<FlightStatusTransitionDto>>(flightStatusTransitions);
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

        var flightStatusTransitions = await _uow.FlightStatusTransitions.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.FlightStatusTransitions.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<FlightStatusTransitionDto>>(flightStatusTransitions);

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
        var total = await _uow.FlightStatusTransitions.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FlightStatusTransitionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightStatusTransitionDto>> GetById(int id, CancellationToken ct)
    {
        var flightStatusTransition = await _uow.FlightStatusTransitions.GetByIdAsync(id, ct);
        if (flightStatusTransition is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FlightStatusTransitionDto>(flightStatusTransition);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FlightStatusTransitionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateFlightStatusTransitionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateFlightStatusTransition>(request);
        var id = await _sender.Send(command, ct);
        var flightStatusTransition = await _uow.FlightStatusTransitions.GetByIdAsync(id, ct);
        if (flightStatusTransition is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FlightStatusTransitionDto>(flightStatusTransition);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightStatusTransitionRequest request, CancellationToken ct)
    {
        var existing = await _uow.FlightStatusTransitions.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateFlightStatusTransition>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var flightStatusTransition = await _uow.FlightStatusTransitions.GetByIdAsync(id, ct);
        if (flightStatusTransition is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteFlightStatusTransition(id), ct);

        return NoContent();
    }
}
