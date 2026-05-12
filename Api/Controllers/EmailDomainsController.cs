using Api.Dtos.EmailDomains;
using Application.Abstractions;
using Application.UseCase.EmailDomains;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class EmailDomainsController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public EmailDomainsController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EmailDomainDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EmailDomainDto>>> GetAll(CancellationToken ct)
    {
        var emailDomains = await _uow.EmailDomains.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<EmailDomainDto>>(emailDomains);
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

        var emailDomains = await _uow.EmailDomains.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.EmailDomains.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<EmailDomainDto>>(emailDomains);

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
        var total = await _uow.EmailDomains.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EmailDomainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailDomainDto>> GetById(int id, CancellationToken ct)
    {
        var emailDomain = await _uow.EmailDomains.GetByIdAsync(id, ct);
        if (emailDomain is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<EmailDomainDto>(emailDomain);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmailDomainDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateEmailDomainRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateEmailDomain>(request);
        var id = await _sender.Send(command, ct);
        var emailDomain = await _uow.EmailDomains.GetByIdAsync(id, ct);
        if (emailDomain is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<EmailDomainDto>(emailDomain);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmailDomainRequest request, CancellationToken ct)
    {
        var existing = await _uow.EmailDomains.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateEmailDomain>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var emailDomain = await _uow.EmailDomains.GetByIdAsync(id, ct);
        if (emailDomain is null)
        {
            return NotFound();
        }

        await _sender.Send(new DeleteEmailDomain(id), ct);

        return NoContent();
    }
}
