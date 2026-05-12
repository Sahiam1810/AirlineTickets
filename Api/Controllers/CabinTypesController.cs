using Api.Dtos.CabinTypes;
using Application.Abstractions;
using Application.UseCase.CabinTypes;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class CabinTypesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CabinTypesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CabinTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CabinTypeDto>>> GetAll(CancellationToken ct)
    {
        var cabinTypes = await _uow.CabinTypes.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<CabinTypeDto>>(cabinTypes);
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

        var cabinTypes = await _uow.CabinTypes.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.CabinTypes.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<CabinTypeDto>>(cabinTypes);

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
        var total = await _uow.CabinTypes.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CabinTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CabinTypeDto>> GetById(int id, CancellationToken ct)
    {
        var cabinType = await _uow.CabinTypes.GetByIdAsync(id, ct);
        if (cabinType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CabinTypeDto>(cabinType);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CabinTypeDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCabinTypeRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCabinType>(request);
        var id = await _sender.Send(command, ct);
        var cabinType = await _uow.CabinTypes.GetByIdAsync(id, ct);
        if (cabinType is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CabinTypeDto>(cabinType);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCabinTypeRequest request, CancellationToken ct)
    {
        var existing = await _uow.CabinTypes.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateCabinType>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var cabinType = await _uow.CabinTypes.GetByIdAsync(id, ct);
        if (cabinType is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteCabinType(id), ct);

        return NoContent();
    }
}
