using Api.Dtos.CardIssuers;
using Application.Abstractions;
using Application.UseCase.CardIssuers;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public sealed class CardIssuersController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CardIssuersController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CardIssuerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CardIssuerDto>>> GetAll(CancellationToken ct)
    {
        var cardIssuers = await _uow.CardIssuers.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<CardIssuerDto>>(cardIssuers);
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

        var cardIssuers = await _uow.CardIssuers.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.CardIssuers.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<CardIssuerDto>>(cardIssuers);

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
        var total = await _uow.CardIssuers.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CardIssuerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CardIssuerDto>> GetById(int id, CancellationToken ct)
    {
        var cardIssuer = await _uow.CardIssuers.GetByIdAsync(id, ct);
        if (cardIssuer is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CardIssuerDto>(cardIssuer);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CardIssuerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCardIssuerRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateCardIssuer>(request);
        var id = await _sender.Send(command, ct);
        var cardIssuer = await _uow.CardIssuers.GetByIdAsync(id, ct);
        if (cardIssuer is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<CardIssuerDto>(cardIssuer);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCardIssuerRequest request, CancellationToken ct)
    {
        var existing = await _uow.CardIssuers.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdateCardIssuer>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeleteCardIssuer(id);
        try
        {
            await _sender.Send(command, ct);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
