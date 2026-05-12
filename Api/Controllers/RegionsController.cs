using Api.Dtos.Regions;
using Application.Abstractions;
using Application.UseCase.Regions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class RegionsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public RegionsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RegionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RegionDto>>> GetAll(CancellationToken ct)
    {
        var regions = await _uow.Regions.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<RegionDto>>(regions);
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

        var regions = await _uow.Regions.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Regions.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<RegionDto>>(regions);

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
        var total = await _uow.Regions.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RegionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RegionDto>> GetById(int id, CancellationToken ct)
    {
        var region = await _uow.Regions.GetByIdAsync(id, ct);
        if (region is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<RegionDto>(region);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RegionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRegionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateRegion>(request);
        var id = await _sender.Send(command, ct);
        var region = await _uow.Regions.GetByIdAsync(id, ct);
        if (region is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<RegionDto>(region);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRegionRequest request, CancellationToken ct)
    {
        var existing = await _uow.Regions.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateRegion>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var region = await _uow.Regions.GetByIdAsync(id, ct);
        if (region is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteRegion(id), ct);

        return NoContent();
    }
}
