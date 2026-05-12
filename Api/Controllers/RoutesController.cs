using Api.Dtos.Routes;
using Application.Abstractions;
using Application.UseCase.Routes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class RoutesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public RoutesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RouteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RouteDto>>> GetAll(CancellationToken ct)
    {
        var routes = await _uow.Routes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<RouteDto>>(routes);
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

        var routes = await _uow.Routes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Routes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<RouteDto>>(routes);

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
        var total = await _uow.Routes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RouteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RouteDto>> GetById(int id, CancellationToken ct)
    {
        var route = await _uow.Routes.GetByIdAsync(id, ct);
        if (route is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<RouteDto>(route);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RouteDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRouteRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateRoute>(request);
        var id = await _sender.Send(command, ct);
        var route = await _uow.Routes.GetByIdAsync(id, ct);
        if (route is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<RouteDto>(route);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRouteRequest request, CancellationToken ct)
    {
        var existing = await _uow.Routes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateRoute>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var route = await _uow.Routes.GetByIdAsync(id, ct);
        if (route is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteRoute(id), ct);

        return NoContent();
    }
}
