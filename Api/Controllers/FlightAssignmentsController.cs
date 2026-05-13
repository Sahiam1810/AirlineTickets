using Api.Dtos.FlightAssignments;
using Application.Abstractions;
using Application.UseCase.FlightAssignments;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class FlightAssignmentsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public FlightAssignmentsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FlightAssignmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<FlightAssignmentDto>>> GetAll(CancellationToken ct)
    {
        var flightAssignments = await _uow.FlightAssignments.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<FlightAssignmentDto>>(flightAssignments);
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

        var flightAssignments = await _uow.FlightAssignments.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.FlightAssignments.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<FlightAssignmentDto>>(flightAssignments);

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
        var total = await _uow.FlightAssignments.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FlightAssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightAssignmentDto>> GetById(int id, CancellationToken ct)
    {
        var flightAssignment = await _uow.FlightAssignments.GetByIdAsync(id, ct);
        if (flightAssignment is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FlightAssignmentDto>(flightAssignment);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(FlightAssignmentDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateFlightAssignmentRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateFlightAssignment>(request);
        var id = await _sender.Send(command, ct);
        var flightAssignment = await _uow.FlightAssignments.GetByIdAsync(id, ct);
        if (flightAssignment is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<FlightAssignmentDto>(flightAssignment);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightAssignmentRequest request, CancellationToken ct)
    {
        var existing = await _uow.FlightAssignments.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateFlightAssignment>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var flightAssignment = await _uow.FlightAssignments.GetByIdAsync(id, ct);
        if (flightAssignment is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteFlightAssignment(id), ct);

        return NoContent();
    }
}
