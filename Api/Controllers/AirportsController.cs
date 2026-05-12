using Api.Dtos.Airports;
using Application.Abstractions;
using Application.UseCase.Airports;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class AirportsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AirportsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AirportDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AirportDto>>> GetAll(CancellationToken ct)
    {
        var airports = await _uow.Airports.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<AirportDto>>(airports);
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

        var airports = await _uow.Airports.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Airports.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<AirportDto>>(airports);

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
        var total = await _uow.Airports.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportDto>> GetById(int id, CancellationToken ct)
    {
        var airport = await _uow.Airports.GetByIdAsync(id, ct);
        if (airport is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AirportDto>(airport);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateAirportRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateAirport>(request);
        var id = await _sender.Send(command, ct);
        var airport = await _uow.Airports.GetByIdAsync(id, ct);
        if (airport is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<AirportDto>(airport);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAirportRequest request, CancellationToken ct)
    {
        var existing = await _uow.Airports.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateAirport>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var airport = await _uow.Airports.GetByIdAsync(id, ct);
        if (airport is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteAirport(id), ct);

        return NoContent();
    }
}
