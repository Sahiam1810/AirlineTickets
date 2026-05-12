using Api.Dtos.RouteStops;
using Application.Abstractions;
using Application.UseCase.RouteStops;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class RouteStopsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public RouteStopsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RouteStopDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RouteStopDto>>> GetAll(CancellationToken ct)
    {
        var routeStops = await _uow.RouteStops.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<RouteStopDto>>(routeStops);
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

        var routeStops = await _uow.RouteStops.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.RouteStops.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<RouteStopDto>>(routeStops);

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
        var total = await _uow.RouteStops.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RouteStopDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RouteStopDto>> GetById(int id, CancellationToken ct)
    {
        var routeStop = await _uow.RouteStops.GetByIdAsync(id, ct);
        if (routeStop is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<RouteStopDto>(routeStop);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RouteStopDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRouteStopRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateRouteStop>(request);
        var id = await _sender.Send(command, ct);
        var routeStop = await _uow.RouteStops.GetByIdAsync(id, ct);
        if (routeStop is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<RouteStopDto>(routeStop);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRouteStopRequest request, CancellationToken ct)
    {
        var existing = await _uow.RouteStops.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateRouteStop>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var routeStop = await _uow.RouteStops.GetByIdAsync(id, ct);
        if (routeStop is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteRouteStop(id), ct);

        return NoContent();
    }
}
