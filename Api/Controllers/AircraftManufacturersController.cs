using Api.Dtos.AircraftManufacturers;
using Application.Abstractions;
using Application.UseCase.AircraftManufacturers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AircraftManufacturersController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AircraftManufacturersController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AircraftManufacturerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AircraftManufacturerDto>>> GetAll(CancellationToken ct)
    {
        var aircraftManufacturers = await _uow.AircraftManufacturers.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AircraftManufacturerDto>>(aircraftManufacturers);
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

        var aircraftManufacturers = await _uow.AircraftManufacturers.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.AircraftManufacturers.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AircraftManufacturerDto>>(aircraftManufacturers);

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
        var total = await _uow.AircraftManufacturers.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AircraftManufacturerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AircraftManufacturerDto>> GetById(int id, CancellationToken ct)
    {
        var aircraftManufacturer = await _uow.AircraftManufacturers.GetByIdAsync(id, ct);
        if (aircraftManufacturer is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AircraftManufacturerDto>(aircraftManufacturer);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AircraftManufacturerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAircraftManufacturerRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAircraftManufacturer>(request);
        var id = await _sender.Send(command, ct);
        var aircraftManufacturer = await _uow.AircraftManufacturers.GetByIdAsync(id, ct);
        if (aircraftManufacturer is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AircraftManufacturerDto>(aircraftManufacturer);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAircraftManufacturerRequest request, CancellationToken ct)
    {
        var existing = await _uow.AircraftManufacturers.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAircraftManufacturer>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var aircraftManufacturer = await _uow.AircraftManufacturers.GetByIdAsync(id, ct);
        if (aircraftManufacturer is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAircraftManufacturer(id), ct);

        return NoContent();
    }
}
