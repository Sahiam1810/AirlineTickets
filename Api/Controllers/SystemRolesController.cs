using Api.Dtos.SystemRoles;
using Application.Abstractions;
using Application.UseCase.SystemRoles;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class SystemRolesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public SystemRolesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<SystemRoleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SystemRoleDto>>> GetAll(CancellationToken ct)
    {
        var roles = await _uow.SystemRoles.GetAllAsync(ct);
        return Ok(_mapper.Map<IReadOnlyList<SystemRoleDto>>(roles));
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

        var roles = await _uow.SystemRoles.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.SystemRoles.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<SystemRoleDto>>(roles);

        return Ok(new { page, pageSize, total, items });
    }

    [HttpGet("count")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> Count([FromQuery] string? search = null, CancellationToken ct = default)
    {
        var total = await _uow.SystemRoles.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SystemRoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SystemRoleDto>> GetById(int id, CancellationToken ct)
    {
        var role = await _uow.SystemRoles.GetByIdAsync(id, ct);
        if (role is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<SystemRoleDto>(role));
    }

    [HttpPost]
    [ProducesResponseType(typeof(SystemRoleDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateSystemRoleRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateSystemRole>(request);
        var id = await _sender.Send(command, ct);
        var role = await _uow.SystemRoles.GetByIdAsync(id, ct);
        if (role is null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(GetById), new { id }, _mapper.Map<SystemRoleDto>(role));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSystemRoleRequest request, CancellationToken ct)
    {
        var existing = await _uow.SystemRoles.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateSystemRole>(request) with { Id = id };
        await _sender.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var existing = await _uow.SystemRoles.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = new DeleteSystemRole(id);
        await _sender.Send(command, ct);
        return NoContent();
    }
}
