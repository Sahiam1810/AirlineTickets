using Api.Dtos.Permissions;
using Application.Abstractions;
using Application.UseCase.Permissions;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class PermissionsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PermissionsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PermissionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PermissionDto>>> GetAll(CancellationToken ct)
    {
        var permissions = await _uow.Permissions.GetAllAsync(ct);
        return Ok(_mapper.Map<IReadOnlyList<PermissionDto>>(permissions));
    }

    [HttpGet("paged")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var permissions = await _uow.Permissions.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Permissions.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PermissionDto>>(permissions);

        return Ok(new { page, pageSize, total, items });
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count([FromQuery] string? search = null, CancellationToken ct = default)
    {
        var total = await _uow.Permissions.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PermissionDto>> GetById(int id, CancellationToken ct)
    {
        var permission = await _uow.Permissions.GetByIdAsync(id, ct);
        if (permission is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PermissionDto>(permission));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PermissionDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePermissionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePermission>(request);
        var id = await _sender.Send(command, ct);
        var permission = await _uow.Permissions.GetByIdAsync(id, ct);
        if (permission is null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(GetById), new { id }, _mapper.Map<PermissionDto>(permission));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionRequest request, CancellationToken ct)
    {
        var existing = await _uow.Permissions.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePermission>(request) with { Id = id };
        await _sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var existing = await _uow.Permissions.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = new DeletePermission(id);
        await _sender.Send(command, ct);
        return NoContent();
    }
}
