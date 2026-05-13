using Api.Dtos.PaymentStates;
using Application.Abstractions;
using Application.UseCase.PaymentStates;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Controllers;

public sealed class PaymentStatesController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PaymentStatesController(IUnitOfWork uow, ISender sender, IMapper mapper)
    {
        _uow = uow;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentStateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PaymentStateDto>>> GetAll(CancellationToken ct)
    {
        var paymentStates = await _uow.PaymentStates.GetAllAsync(ct);
        var result = _mapper.Map<IReadOnlyList<PaymentStateDto>>(paymentStates);
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

        var paymentStates = await _uow.PaymentStates.GetPagedAsync(page, pageSize, search, ct);
        var total = await _uow.PaymentStates.CountAsync(search, ct);
        var items = _mapper.Map<IReadOnlyList<PaymentStateDto>>(paymentStates);

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
        var total = await _uow.PaymentStates.CountAsync(search, ct);
        return Ok(new { total });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PaymentStateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentStateDto>> GetById(int id, CancellationToken ct)
    {
        var paymentState = await _uow.PaymentStates.GetByIdAsync(id, ct);
        if (paymentState is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PaymentStateDto>(paymentState);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentStateDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentStateRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreatePaymentState>(request);
        var id = await _sender.Send(command, ct);
        var paymentState = await _uow.PaymentStates.GetByIdAsync(id, ct);
        if (paymentState is null)
        {
            return NotFound();
        }

        var result = _mapper.Map<PaymentStateDto>(paymentState);
        return CreatedAtAction(nameof(GetById), new { id }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentStateRequest request, CancellationToken ct)
    {
        var existing = await _uow.PaymentStates.GetByIdAsync(id, ct);
        if (existing is null)
        {
            return NotFound();
        }

        var command = _mapper.Map<UpdatePaymentState>(request) with { Id = id };
        await _sender.Send(command, ct);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var command = new DeletePaymentState(id);
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
