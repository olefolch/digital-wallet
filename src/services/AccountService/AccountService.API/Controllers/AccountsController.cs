using AccountService.Application.Features.Accounts.Commands.CreateAccount;
using AccountService.Application.Features.Accounts.Commands.Deposit;
using AccountService.Application.Features.Accounts.Commands.Withdraw;
using AccountService.Application.Features.Accounts.Queries.GetAccount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers;

public record AmountRequest(decimal Amount);

[ApiController]
[Route("api/v1/accounts")]
public class AccountsController(IMediator mediator) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
    {
        var accountId = await mediator.Send(command);

        return CreatedAtAction(nameof(Create), new { id = accountId }, new { id = accountId });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var account = await mediator.Send(new GetAccountQuery(id));

        return Ok(new { account });
    }

    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, [FromBody] AmountRequest request)
    {
        await mediator.Send(new DepositCommand(id, request.Amount));
        return NoContent();
    }

    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(Guid id, [FromBody] AmountRequest request)
    {
        await mediator.Send(new WithdrawCommand(id, request.Amount));
        return NoContent();
    }
}