using Api.Dtos.Clients;
using Application.Abstractions;
using Application.UseCase.Clients;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class ClientsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public ClientsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClientDto>>> GetAll(CancellationToken ct)
    {
        var clients = await _uow.Clients.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<ClientDto>>(clients);
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

        var clients = await _uow.Clients.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.Clients.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<ClientDto>>(clients);

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
        var total = await _uow.Clients.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDto>> GetById(int id, CancellationToken ct)
    {
        var client = await _uow.Clients.GetByIdAsync(id, ct);
        if (client is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ClientDto>(client);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateClient>(request);
        var id = await _sender.Send(command, ct);
        var client = await _uow.Clients.GetByIdAsync(id, ct);
        if (client is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<ClientDto>(client);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var client = await _uow.Clients.GetByIdAsync(id, ct);
        if (client is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteClient(id), ct);

        return NoContent();
    }
}
