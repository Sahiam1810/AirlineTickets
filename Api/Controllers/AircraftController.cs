using Api.Dtos.Aircraft;
using Application.Abstractions;
using Application.UseCase.Aircraft;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AircraftController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AircraftController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AircraftDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AircraftDto>>> GetAll(CancellationToken ct)
    {
        var aircraft = await _uow.Aircraft.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AircraftDto>>(aircraft);
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

        var aircraft = await _uow.Aircraft.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Aircraft.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AircraftDto>>(aircraft);

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
        var total = await _uow.Aircraft.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AircraftDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AircraftDto>> GetById(int id, CancellationToken ct)
    {
        var aircraft = await _uow.Aircraft.GetByIdAsync(id, ct);
        if (aircraft is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AircraftDto>(aircraft);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AircraftDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAircraftRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAircraft>(request);
        var id = await _sender.Send(command, ct);
        var aircraft = await _uow.Aircraft.GetByIdAsync(id, ct);
        if (aircraft is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AircraftDto>(aircraft);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAircraftRequest request, CancellationToken ct)
    {
        var existing = await _uow.Aircraft.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAircraft>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var aircraft = await _uow.Aircraft.GetByIdAsync(id, ct);
        if (aircraft is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAircraft(id), ct);

        return NoContent();
    }
}
